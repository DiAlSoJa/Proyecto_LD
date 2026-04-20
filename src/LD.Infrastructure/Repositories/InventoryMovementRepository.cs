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

        public async Task<List<InventoryMovement>> GetAllWithRelationsAsync()
        {
            return await _context.InventoryMovements
                .Include(x => x.Product)
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x => x.Location)
                .Include(x => x.StandardLabel)
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
