namespace MyDiary.Domain.Repositories;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}