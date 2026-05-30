using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        public readonly LdProyectDbContext _context;

        public InventoryMovementRepository(LdProyectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(InventoryMovement newModel)
        {
            try
            {
                if (newModel == null)
                    return false;

                await _context.InventoryMovements.AddAsync(newModel);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(InventoryMovement modelToDelete)
        {
            try
            {
                if (modelToDelete == null)
                    return false;

                _context.InventoryMovements.Remove(modelToDelete);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<InventoryMovement>> GetAllWithRelationsAsync(int? standardId = null, string? standardIdCode = null)
        {
            IQueryable<InventoryMovement> query = _context.InventoryMovements
                .Include(x => x.Product)
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x => x.Location)
                    .ThenInclude(x => x!.Warehouse)
                .Include(x => x.StandardLabel);

            var standardIdCodeText = standardIdCode?.Trim();
            var hasStandardIdCodeText = !string.IsNullOrWhiteSpace(standardIdCodeText);

            if (hasStandardIdCodeText)
            {
                query = query.Where(x =>
                    x.StandardId.HasValue &&
                    _context.StandardLabels.Any(label =>
                        label.StandarId == x.StandardId.Value &&
                        label.StandarIdStr == standardIdCodeText));
            }
            else if (standardId.HasValue)
            {
                query = query.Where(x => x.StandardId == standardId.Value);
            }

            return await query
                .ToListAsync();
        }

        public async Task<InventoryMovement?> GetByIdAsync(int id)
        {
            return await _context.InventoryMovements
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x => x.Location)
                    .ThenInclude(x => x!.Warehouse)
                .Include(x => x.StandardLabel)
                .FirstOrDefaultAsync(x => x.MovementId == id);
        }

        public async Task<InventoryMovement?> GetByIdAsync(string id)
        {
            return await _context.InventoryMovements
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x => x.Location)
                    .ThenInclude(x => x!.Warehouse)
                .Include(x => x.StandardLabel)
                .FirstOrDefaultAsync(x => x.MovementId.ToString() == id);
        }

        public async Task<List<InventoryMovement>?> GetManyAsync()
        {
            return await _context.InventoryMovements
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(InventoryMovement modelToUpdate)
        {
            try
            {
                _context.InventoryMovements.Update(modelToUpdate);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
