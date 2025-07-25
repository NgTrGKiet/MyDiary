﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyDiary.Application.Diary.Dtos;
using MyDiary.Domain.Entities;
using MyDiary.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Queries.GetDiaryById
{
    public class GetDiaryByIdQueryHandler(IMapper mapper, 
        IDiaryRepository diaryRepository,
        ILogger<GetDiaryByIdQueryHandler> logger) : IRequestHandler<GetDiaryByIdQuery, DiaryDtos>
    {
        public async Task<DiaryDtos> Handle(GetDiaryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null || request.diaryId == null) 
                throw new ArgumentNullException(nameof(request));
            logger.LogInformation("Get diary: {diaryId}", request.diaryId);

            var diary = await diaryRepository.GetById(request.diaryId) ?? throw new Exception();
            var diaryDto = mapper.Map<DiaryDtos>(diary);
            return diaryDto;
        }
    }
}
