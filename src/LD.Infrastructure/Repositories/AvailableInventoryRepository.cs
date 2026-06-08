using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class AvailableInventoryRepository : IAvailableInventoryRepository
{
    private readonly LdProyectDbContext _context;

    public AvailableInventoryRepository(LdProyectDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(AvailableInventory newModel)
    {
        try
        {
            if (newModel == null)
                return false;

            await _context.AvailableInventories.AddAsync(newModel);
            return await _context.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(AvailableInventory modelToDelete)
    {
        try
        {
            if (modelToDelete == null)
                return false;

            _context.AvailableInventories.Remove(modelToDelete);
            return await _context.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<AvailableInventory>> GetAllWithRelationsAsync(int? standardId = null, string? standardIdCode = null)
    {
        IQueryable<AvailableInventory> query = _context.AvailableInventories
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Client)
            .Include(x => x.Project)
            .Include(x => x.Location)
                .ThenInclude(x => x!.Warehouse)
            .Include(x => x.StandardLabel);

        if (standardId.HasValue)
        {
            query = query.Where(x => x.StandardId == standardId.Value);
        }

        if (!string.IsNullOrWhiteSpace(standardIdCode))
        {
            var normalizedStandardIdCode = standardIdCode.Trim();
            var standardIds = await _context.StandardLabels
                .AsNoTracking()
                .Where(x => x.StandarIdStr == normalizedStandardIdCode)
                .Select(x => x.StandarId)
                .ToListAsync();

            if (standardIds.Count == 0)
                return new List<AvailableInventory>();

            query = query.Where(x => x.StandardId.HasValue && standardIds.Contains(x.StandardId.Value));
        }

        return await query
            .ToListAsync();
    }

    public async Task<AvailableInventory?> GetByIdAsync(int id)
    {
        return await _context.AvailableInventories
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Client)
            .Include(x => x.Project)
            .Include(x => x.Location)
                .ThenInclude(x => x!.Warehouse)
            .Include(x => x.StandardLabel)
            .FirstOrDefaultAsync(x => x.AvailableInventoryId == id);
    }

    public async Task<AvailableInventory?> GetByIdAsync(string id)
    {
        return await _context.AvailableInventories
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Client)
            .Include(x => x.Project)
            .Include(x => x.Location)
                .ThenInclude(x => x!.Warehouse)
            .Include(x => x.StandardLabel)
            .FirstOrDefaultAsync(x => x.AvailableInventoryId.ToString() == id);
    }

    public async Task<List<AvailableInventory>?> GetManyAsync()
    {
        return await _context.AvailableInventories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(AvailableInventory modelToUpdate)
    {
        try
        {
            _context.AvailableInventories.Update(modelToUpdate);
            return await _context.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }
}
