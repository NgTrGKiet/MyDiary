using MyDiary.Domain.Entities;

namespace MyDiary.Domain.Repositories;

public interface IDiaryRepository
{
    Task<DiaryEntity> GetById(Guid diaryId);

    Task<Guid> Create(DiaryEntity entity);

    Task Delete(DiaryEntity entity);

    Task Update(DiaryEntity entity);
}
