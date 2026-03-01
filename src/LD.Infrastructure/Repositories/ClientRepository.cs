using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public readonly LdProyectDbContext _context;
        public ClientRepository(LdProyectDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(Client newModel)
        {
            try
            {
                if (newModel == null)
                    return false;

                await _context.Clients.AddAsync(newModel);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients.Include(c=>c.ClientFiscalData).FirstOrDefaultAsync(c=>c.ClientId == id);
        }

        public async Task<List<Client>?> GetManyAsync()
        {
            return await _context.Clients.Include(c => c.ClientFiscalData).ToListAsync();
        }

 
        public async Task<bool> UpdateAsync(Client entity)
        {
            try
            {
                _context.Clients.Update(entity);

                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    
    }
}
