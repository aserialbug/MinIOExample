using Microsoft.EntityFrameworkCore.Storage;
using MinIOExample.Application.Interfaces;

namespace MinIOExample.Infrastructure.Context;

public class BusinessTransactionContext : IBusinessTransactionContext
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public BusinessTransactionContext(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken token = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(token);
    }

    public async Task CommitTransactionAsync(CancellationToken token = default)
    {
        if (_transaction != null)
        {
           await _context.SaveChangesAsync(token);
           await _transaction.CommitAsync(token);
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken token = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(token);
        }
    }
}