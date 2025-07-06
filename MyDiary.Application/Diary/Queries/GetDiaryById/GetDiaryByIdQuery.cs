using MediatR;
using MyDiary.Application.Diary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Queries.GetDiaryById
{
    public class GetDiaryByIdQuery(Guid diaryId) : IRequest<DiaryDtos>
    {
        public Guid diaryId { get; } = diaryId;
    }
}
