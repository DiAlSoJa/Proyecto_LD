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
            var standardIdText = standardId.Value.ToString();
            query = query.Where(x =>
                x.StandardId == standardId.Value ||
                (x.StandardLabel != null && x.StandardLabel.StandarIdStr == standardIdText));
        }

        if (!string.IsNullOrWhiteSpace(standardIdCode))
        {
            var normalizedStandardIdCode = standardIdCode.Trim();
            query = query.Where(x =>
                x.StandardLabel != null &&
                x.StandardLabel.StandarIdStr == normalizedStandardIdCode);
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
