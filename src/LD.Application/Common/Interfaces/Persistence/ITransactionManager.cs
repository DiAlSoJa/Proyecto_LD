namespace LD.Application.Common.Interfaces.Persistence;

public interface ITransactionManager
{
    Task<IAppTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
