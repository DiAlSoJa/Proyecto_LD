using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
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

        public Task<Client?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>?> GetManyAsync()
        {
            throw new NotImplementedException();
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
