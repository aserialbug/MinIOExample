namespace MinIOExample.Application.Interfaces;

public interface IRepository<TEntity, TId>
{
    public Task<TEntity> this[TId id] { get; }
    public Task AddAsync(TEntity entity, CancellationToken token = default);
    public Task RemoveAsync(TEntity entity, CancellationToken token = default);
}