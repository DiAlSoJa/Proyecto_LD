using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class KittingFolioCaptureRepository : Repository<KittingFolioCapture>, IKittingFolioCaptureRepository
{
    private readonly LdProyectDbContext _dbContext;

    public KittingFolioCaptureRepository(LdProyectDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<List<KittingFolioCapture>> GetManyWithRelationsAsync(int? clientId = null, int? projectId = null)
    {
        var query = _dbContext.KittingFolioCaptures
            .AsNoTracking()
            .Include(x => x.Kitting)
                .ThenInclude(x => x!.Client)
            .Include(x => x.Kitting)
                .ThenInclude(x => x!.Project)
            .Include(x => x.KittingDetail)
            .AsQueryable();

        if (clientId.HasValue && clientId.Value > 0)
        {
            query = query.Where(x => x.Kitting != null && x.Kitting.ClientId == clientId.Value);
        }

        if (projectId.HasValue && projectId.Value > 0)
        {
            query = query.Where(x => x.Kitting != null && x.Kitting.ProjectId == projectId.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.KittingFolioCaptureId)
            .ToListAsync();
    }

    public async Task<KittingFolioCapture?> GetByIdWithRelationsAsync(int id)
    {
        return await _dbContext.KittingFolioCaptures
            .AsNoTracking()
            .Include(x => x.Kitting)
                .ThenInclude(x => x!.Client)
            .Include(x => x.Kitting)
                .ThenInclude(x => x!.Project)
            .Include(x => x.KittingDetail)
            .FirstOrDefaultAsync(x => x.KittingFolioCaptureId == id);
    }
}
