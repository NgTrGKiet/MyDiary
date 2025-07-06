using AutoMapper;
using MediatR;
using MyDiary.Application.Exceptions;
using MyDiary.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Commands.UpdateDiary
{
    public class UpdateDiaryCommandHandler(IDiaryRepository diaryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateDiaryCommand>
    {
        public async Task Handle(UpdateDiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var diary = await diaryRepository.GetById(request.DiaryId);
                if (diary == null)
                    throw new NotFoundException(nameof(diary), request.DiaryId.ToString());

                await unitOfWork.BeginTransactionAsync();
                diary.DiaryTitle = request.DiaryTitle;
                diary.DiaryStory = request.DiaryStory;
                await diaryRepository.Update(diary);
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
