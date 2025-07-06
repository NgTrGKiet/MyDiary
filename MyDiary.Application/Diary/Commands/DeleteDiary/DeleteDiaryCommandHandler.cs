using MediatR;
using MyDiary.Application.Exceptions;
using MyDiary.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Commands.DeleteDiary
{
    public class DeleteDiaryCommandHandler(IDiaryRepository diaryRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDiaryCommand>
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
