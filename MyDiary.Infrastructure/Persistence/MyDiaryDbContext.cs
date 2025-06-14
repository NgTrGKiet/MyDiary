using Microsoft.EntityFrameworkCore;

namespace MyDiary.Infrastructure.Persistence;

public class MyDiaryDbContext : DbContext
{
    public MyDiaryDbContext(DbContextOptions<MyDiaryDbContext> options) : base(options){}
}