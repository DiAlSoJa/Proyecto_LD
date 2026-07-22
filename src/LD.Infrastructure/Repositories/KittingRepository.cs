using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class KittingRepository : IKittingRepository
{
    private readonly LdProyectDbContext _context;

    public KittingRepository(LdProyectDbContext context)
    {
        _context = context;
    }

    async Task<bool> IRepository<Kitting>.CreateAsync(Kitting newModel)
    {
        return await CreateWithSequenceAsync(newModel);
    }

    public async Task<bool> CreateWithSequenceAsync(Kitting entity)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var project = await _context.Set<Project>()
                .FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

            if (project is null)
                throw new Exception("No se encontró el proyecto.");

            if (string.IsNullOrWhiteSpace(project.KittingPrefix))
                throw new Exception("El proyecto no tiene configurado el prefijo de Kit.");

            var currentNumber = 1;
            if (!string.IsNullOrWhiteSpace(project.KittingNumber) && int.TryParse(project.KittingNumber, out var parsedNumber))
                currentNumber = parsedNumber;

            entity.KittingCode = $"{project.KittingPrefix}{currentNumber:D5}";
            entity.PreKittingCode = entity.KittingCode;

            _context.Set<Kitting>().Add(entity);

            project.KittingNumber = (currentNumber + 1).ToString();
            _context.Set<Project>().Update(project);

            var result = await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return result > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Kitting>> GetKittingByClientAsync(int clientId, int projectId)
    {
        return await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Project)
                .ThenInclude(x => x.Warehouse)
            .Where(x => x.ClientId == clientId && x.ProjectId == projectId)
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();
    }

    public async Task<List<Kitting>> GetAllWithRelationsAsync()
    {
        return await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Project)
                .ThenInclude(x => x.Warehouse)
            .Include(x => x.Client)
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();
    }

    async Task<Kitting?> IRepository<Kitting>.GetByIdAsync(int id)
    {
        return await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Project)
                .ThenInclude(x => x.Warehouse)
            .FirstOrDefaultAsync(x => x.KittingId == id);
    }

    Task<Kitting?> IRepository<Kitting>.GetByIdAsync(string id)
    {
        if (!int.TryParse(id, out var kittingId))
            return Task.FromResult<Kitting?>(null);

        return ((IRepository<Kitting>)this).GetByIdAsync(kittingId);
    }

    async Task<List<Kitting>?> IRepository<Kitting>.GetManyAsync()
    {
        return await _context.Kittings
            .AsNoTracking()
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();
    }

    async Task<bool> IRepository<Kitting>.UpdateAsync(Kitting modelToUpdate)
    {
        _context.Kittings.Update(modelToUpdate);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Kitting modelToDelete)
    {
        _context.Kittings.Remove(modelToDelete);
        return await _context.SaveChangesAsync() > 0;
    }
}
