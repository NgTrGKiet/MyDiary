using Microsoft.AspNetCore.Identity;

namespace MyDiary.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}