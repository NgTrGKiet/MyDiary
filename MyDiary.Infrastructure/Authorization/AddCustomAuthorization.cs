using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Infrastructure.Authorization
{
    public static class AddCustomAuthorization
    {
        public static IServiceCollection CustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy("Admin", policy => policy.RequireRole("Administrator"));
                option.AddPolicy("User", policy => policy.RequireRole("User"));
            });
            return services;
        }
    }
}
