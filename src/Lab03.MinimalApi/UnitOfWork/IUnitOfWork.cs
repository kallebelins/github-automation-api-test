namespace Lab03.MinimalApi.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
