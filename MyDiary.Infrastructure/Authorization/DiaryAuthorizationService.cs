using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyDiary.Application.Contracts.Identity;
using MyDiary.Domain.Constants;
using MyDiary.Domain.Entities;
using MyDiary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Infrastructure.Authorization
{
    public class DiaryAuthorizationService(ILogger<DiaryAuthorizationService> logger, IHttpContextAccessor httpContextAccessor) : IDiaryAuthorizationService
    {
        public bool Authorize(DiaryEntity diary, ResourceOperation resourceOperation)
        {
            var user = httpContextAccessor?.HttpContext?.User;

            if (user == null)
                throw new InvalidOperationException("User is null");

            if (user.Identity == null || !user.Identity.IsAuthenticated)
                throw new InvalidOperationException("User is null");

            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;

            logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for diary {RestaurantName}",
                    email,
                    resourceOperation,
                    diary.DiaryTitle);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create/read operation - successful authorization");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Read)
                && userId == diary.UserId)
            {
                logger.LogInformation("Restaurant owner - successful authorization");
                return true;
            }

            return false;
        }
    }
}
