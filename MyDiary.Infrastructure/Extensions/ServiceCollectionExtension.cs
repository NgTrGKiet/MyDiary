using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyDiary.Application.Auth;
using MyDiary.Application.Auth.Models;
using MyDiary.Application.Contracts.Identity;
using MyDiary.Domain.Entities;
using MyDiary.Domain.Interfaces;
using MyDiary.Domain.Repositories;
using MyDiary.Infrastructure.Authorization;
using MyDiary.Infrastructure.Persistence;
using MyDiary.Infrastructure.Repositories;

namespace MyDiary.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var connectionString = configuration.GetConnectionString("DiaryDb");

            services.AddDbContext<MyDiaryDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .EnableSensitiveDataLogging()
            );
            
            AddIdentity(services, configuration);
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IDiaryAuthorizationService, DiaryAuthorizationService>();

            services.CustomAuthorization();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<MyDiaryDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthService, AuthService>();
        services.AddScoped<IDiaryRepository, DiaryRepository>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
            };
        });
    }
}