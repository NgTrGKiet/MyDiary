using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Commands.DeleteDiary
{
    public class DeleteDiaryCommand(Guid diaryId) : IRequest
    {
        public Guid DiaryId { get; } = diaryId;
    }
}
