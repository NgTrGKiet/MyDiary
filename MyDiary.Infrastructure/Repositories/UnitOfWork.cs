using Microsoft.EntityFrameworkCore.Storage;
using MyDiary.Domain.Repositories;
using MyDiary.Infrastructure.Persistence;

namespace MyDiary.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDiaryDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(MyDiaryDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null) return;
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction?.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        await _transaction?.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}