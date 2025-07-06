using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyDiary.Domain.Entities;

namespace MyDiary.Infrastructure.Persistence;

public class MyDiaryDbContext : IdentityDbContext<ApplicationUser>
{
    public MyDiaryDbContext(DbContextOptions<MyDiaryDbContext> options) : base(options) { }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<DiaryEntity> Diarys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDiaryDbContext).Assembly);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<DiaryEntity>()
            .HasOne(d => d.User)
            .WithMany(d => d.DiaryEntities)
            .HasForeignKey(d => d.UserId);
    }
}