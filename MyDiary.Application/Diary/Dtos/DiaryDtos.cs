using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Dtos
{
    public class DiaryDtos
    {
        public Guid DiaryId { get; set; }

        public string? DiaryTitle { get; set; }

        public DateTime CreatedTime { get; set; }

        public string? DiaryStory { get; set; }

        public string UserId { get; set; }
    }
}
