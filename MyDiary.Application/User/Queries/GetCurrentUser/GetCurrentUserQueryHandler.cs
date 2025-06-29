using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyDiary.Application.User.Dtos;

namespace MyDiary.Application.User.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(ILogger<GetCurrentUserQueryHandler> logger, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor?.HttpContext?.User;
        
        if(user == null)
            throw new InvalidOperationException("User is null");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return null;
        
        string userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        string email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        
        string name = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

        var roles =  user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        return new UserDto
        {
            Name = name,
            Email = email,
            Username = userId,
            Role = roles.ToString()
        };
    }
}