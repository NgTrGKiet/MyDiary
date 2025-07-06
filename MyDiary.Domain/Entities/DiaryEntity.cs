using System.ComponentModel.DataAnnotations;

namespace MyDiary.Domain.Entities
{
    public class DiaryEntity
    {
        [Key]
        public Guid DiaryId { get; set; }

        public string? DiaryTitle { get; set; }

        public DateTime CreatedTime { get; set; }

        public string? DiaryStory { get; set; }

        public string UserId { get; set; } = default!;

        public ApplicationUser User { get; set; } = default!;

    }
}
