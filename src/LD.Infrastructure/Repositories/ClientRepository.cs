using LD.Application.Common.Interfaces;
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
    public class ClientRepository : IRepository<Client>
    {
        private readonly LdProyectDbContext _dbContext;
        public ClientRepository(LdProyectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Client?> GetById(int id)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c=>c.ClientId == id);
        }

        public async Task<List<Client>?> GetMany()
        {
            return await _dbContext.Clients.ToListAsync();
        }
      
        public async Task<bool> Create(Client newModel)
        {
            await _dbContext.Clients.AddAsync(newModel);
            return await _dbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> Update(Client modelToUpdate)
        {
            _dbContext.Clients.Update(modelToUpdate);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> Archive(int id)
        {
            var client = await GetById(id);
            if (client is null) return false;

            client.Activo = false;
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
