using Microsoft.EntityFrameworkCore;
using MyDiary.Domain.Entities;
using MyDiary.Domain.Repositories;
using MyDiary.Infrastructure.Persistence;

namespace MyDiary.Infrastructure.Repositories
{
    internal class DiaryRepository(MyDiaryDbContext dbContext) : IDiaryRepository
    {
        public async Task<(IEnumerable<DiaryEntity>, int)> GetAllMatchingAsync(string? searchValue, int pageSize, int pageNumber)
        {
            try
            {
                string searchValueLower = searchValue?.ToLower();

                var baseQuery = dbContext.Diarys
                    .Where(d => searchValueLower == null || d.DiaryTitle.ToLower().Contains(searchValueLower))
                    .Skip(pageSize * (pageNumber - 1));

                var totalCount = await baseQuery.CountAsync();

                var diaries = await baseQuery.Take(pageSize).ToListAsync();

                return (diaries, totalCount);

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DiaryEntity> GetById(Guid diaryId)
        {
            try
            {
                DiaryEntity entity = await dbContext.Diarys.FindAsync(diaryId);
                if(entity is not null)
                {
                    return entity;
                }
                return null;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> Create(DiaryEntity entity)
        {
            try
            {
                await dbContext.Diarys.AddAsync(entity);
                return entity.DiaryId;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(DiaryEntity entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (dbContext.Entry(entity).State != EntityState.Unchanged)
                    {
                        dbContext.Entry(entity).CurrentValues.SetValues(entity);
                    }
                });
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(DiaryEntity entity)
        {
            try
            {
                await Task.Run(() =>
                {
                    dbContext.Diarys.Remove(entity);
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
