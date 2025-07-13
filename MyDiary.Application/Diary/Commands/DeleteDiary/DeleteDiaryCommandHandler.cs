using MediatR;
using Microsoft.Extensions.Logging;
using MyDiary.Application.Exceptions;
using MyDiary.Domain.Interfaces;
using MyDiary.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Commands.DeleteDiary
{
    public class DeleteDiaryCommandHandler(IDiaryRepository diaryRepository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteDiaryCommandHandler> logger,
        IDiaryAuthorizationService diaryAuthorizationService) : IRequestHandler<DeleteDiaryCommand>
    {
        public async Task Handle(DeleteDiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null || request.DiaryId == null)
                    throw new ArgumentException("DiaryId is null");

                var diary = await diaryRepository.GetById(request.DiaryId);
                if (diary == null)
                    throw new NotFoundException(nameof(diary), request.DiaryId.ToString());

                logger.LogWarning("Remove diary: {diaryId}", request.DiaryId);

                if (!diaryAuthorizationService.Authorize(diary, Domain.Constants.ResourceOperation.Delete))
                    throw new ForbidException();

                await unitOfWork.BeginTransactionAsync();
                await diaryRepository.Delete(diary);
                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitAsync();
            } catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
