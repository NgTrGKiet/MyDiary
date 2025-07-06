using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyDiary.Application.Auth.Models;
using MyDiary.Application.Contracts.Identity;
using MyDiary.Application.Exceptions;
using MyDiary.Domain.Entities;
using MyDiary.Domain.Repositories;

namespace MyDiary.Application.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    private readonly JwtSettings _jwtSettings;
    
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> jwtSettings, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        try
        {
            _unitOfWork.BeginTransactionAsync();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new NotFoundException($"User with {request.Email} not found.", request.Email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
                throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");

            JwtSecurityToken jwtSecurityToken = await GenerateAccessToken(user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens ??= new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            var response = new AuthResponse
            {
                Id = user.Id,
                AccessToken = accessToken,
                Email = user.Email,
                UserName = user.UserName,
                RefreshToken = refreshToken.Token
            };
            return response;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            Name = request.Name,
            UserName = request.UserName,
            EmailConfirmed = true
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            return new RegistrationResponse() { UserId = user.Id };
        }
        else
        {
            StringBuilder str = new StringBuilder();
            foreach (var err in result.Errors)
            {
                str.AppendFormat("•{0}\n", err.Description);
            }
                
            throw new BadRequestException($"{str}");
        }
    }

    public async Task<AuthResponse> RefreshToken(string refreshToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
    
            if (user == null) 
                throw new UnauthorizedAccessException("You are not authorized to refresh this token.");
        
            var oldRefreshTokens = user.RefreshTokens.First(x => x.Token == refreshToken);
            if(!oldRefreshTokens.IsActive)
                throw new UnauthorizedAccessException("Expired or revoked refresh token.");
        
            oldRefreshTokens.Revoked = DateTime.UtcNow;
            oldRefreshTokens.RevokedByIp = Environment.MachineName ?? "unknown" ;
        
            var newRefreshToken = GenerateRefreshToken();
            var newAccessToken = await GenerateAccessToken(user);
        
            user.RefreshTokens.Add(newRefreshToken);
            
            await _userManager.UpdateAsync(user);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return new AuthResponse
            {
                Id = user.Id,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                Email = user.Email,
                UserName = user.UserName,
                RefreshToken = newRefreshToken.Token
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception(ex.Message);
        }

    }

    private async Task<JwtSecurityToken> GenerateAccessToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        
        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(7),
        };
    }
}