namespace IDENTITY.DOMAIN;

public interface IUnitOfWorkDbContext<TContext> : IAsyncDisposable
{
    /// <summary>
    /// Call save change
    /// </summary>
    /// <returns></returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

}