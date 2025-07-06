using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Commands.UpdateDiary
{
    public class UpdateDiaryCommand : IRequest
    {
        public Guid DiaryId { get; set; }

        public string? DiaryTitle { get; set; }

        public string? DiaryStory { get; set; }
    }
}
