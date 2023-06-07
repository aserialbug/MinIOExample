namespace MinIOExample.Application.Interfaces;

public interface IBusinessTransactionContext
{
    Task BeginTransactionAsync(CancellationToken token = default);
    Task CommitTransactionAsync(CancellationToken token = default);
    Task RollbackTransactionAsync(CancellationToken token = default);
}