using MediatR;

namespace MyDiary.Application.Diary.Commands.CreateDiary
{
    public class CreateDiaryCommand : IRequest<Guid>
    {
        public string? DiaryTitle { get; set; }

        public DateTime CreatedTime { get; set; }

        public string? DiaryStory { get; set; }
    }
}
