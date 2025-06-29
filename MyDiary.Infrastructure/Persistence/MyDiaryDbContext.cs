using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyDiary.Domain.Entities;

namespace MyDiary.Infrastructure.Persistence;

public class MyDiaryDbContext : IdentityDbContext<ApplicationUser>
{
    public MyDiaryDbContext(DbContextOptions<MyDiaryDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDiaryDbContext).Assembly);
        }
}