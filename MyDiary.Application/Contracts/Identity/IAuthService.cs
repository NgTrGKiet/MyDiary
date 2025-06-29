using MyDiary.Application.Auth.Models;

namespace MyDiary.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    
    Task<RegistrationResponse> Register(RegistrationRequest request);
    
    Task<AuthResponse> RefreshToken(string refreshToken);
}