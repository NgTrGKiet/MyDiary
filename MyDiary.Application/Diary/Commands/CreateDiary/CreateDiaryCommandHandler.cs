using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MyDiary.Domain.Entities;
using MyDiary.Domain.Repositories;
using System.Security.Claims;

namespace MyDiary.Application.Diary.Commands.CreateDiary
{
    public class CreateDiaryCommandHandler(IDiaryRepository diaryRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper) : IRequestHandler<CreateDiaryCommand, Guid>
    {
        public async Task<Guid> Handle(CreateDiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                var user = httpContextAccessor?.HttpContext?.User;

                if (user == null)
                    throw new InvalidOperationException("User is null");

                if (user.Identity == null || !user.Identity.IsAuthenticated)
                    throw new InvalidOperationException("User is null");

                string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var diary = mapper.Map<DiaryEntity>(request);
                diary.UserId = userId;
                Guid diaryId = await diaryRepository.Create(diary);
                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitAsync();
                return diaryId;

            } catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
