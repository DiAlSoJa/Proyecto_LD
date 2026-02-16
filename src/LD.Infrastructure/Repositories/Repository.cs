using LD.Application.Common.Interfaces;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly LdProyectDbContext _context;
        public Repository(LdProyectDbContext ldProyectDbContext)
        {
            _context = ldProyectDbContext;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetManyAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> CreateAsync(T newModel)
        {
            await _context.Set<T>().AddAsync(newModel);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T modelToUpdate)
        {
            _context.Set<T>().Update(modelToUpdate);
            return await _context.SaveChangesAsync() > 0;
        }

    }

}
