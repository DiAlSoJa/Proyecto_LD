using LD.Application.Common.Interfaces.Persistence;

namespace LD.Infrastructure.Persistence;

public class TransactionManager : ITransactionManager
{
    private readonly LdProyectDbContext _context;

    public TransactionManager(LdProyectDbContext context)
    {
        _context = context;
    }

    public async Task<IAppTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new AppTransaction(transaction);
    }
}
