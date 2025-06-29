using System.ComponentModel.DataAnnotations;

namespace MyDiary.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string? Token { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= Expires;
    
    public DateTime Expires { get; set; }
    
    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }
    
    public bool IsActive => Revoked == null && !IsExpired;
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }

}