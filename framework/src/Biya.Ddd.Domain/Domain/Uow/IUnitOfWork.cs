namespace Domain.Uow;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default
    );

    Task BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );

    Task CommitTransactionAsync(
        CancellationToken cancellationToken = default
    );

    Task RollbackTransactionAsync(
        CancellationToken cancellationToken = default
    );
}