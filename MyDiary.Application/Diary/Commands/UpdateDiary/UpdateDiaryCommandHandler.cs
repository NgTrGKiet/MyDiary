﻿using AutoMapper;
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

namespace MyDiary.Application.Diary.Commands.UpdateDiary
{
    public class UpdateDiaryCommandHandler(IDiaryRepository diaryRepository, 
        IUnitOfWork unitOfWork,
        ILogger<UpdateDiaryCommandHandler> logger,
        IDiaryAuthorizationService diaryAuthorizationService) : IRequestHandler<UpdateDiaryCommand>
    {
        public async Task Handle(UpdateDiaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var diary = await diaryRepository.GetById(request.DiaryId);
                if (diary == null)
                    throw new NotFoundException(nameof(diary), request.DiaryId.ToString());

                logger.LogWarning("Update diary: {diaryId}", request.DiaryId);

                if (!diaryAuthorizationService.Authorize(diary, Domain.Constants.ResourceOperation.Update))
                    throw new ForbidException();

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
