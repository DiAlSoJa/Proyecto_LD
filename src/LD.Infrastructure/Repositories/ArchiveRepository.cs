using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Common;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Repositories
{
    public class ArchiveRepository<T>
    : IArchiveRepository<T> where T : AuditableEntity
    {
        protected readonly LdProyectDbContext _context;
        public ArchiveRepository(LdProyectDbContext ldProyectDbContext)
        {
            _context = ldProyectDbContext;
        }

        public async Task<bool> ArchiveAsync(int id,bool isActive)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity is null)
                return false;

            entity.IsActive = isActive;

            _context.Set<T>().Update(entity);

            return await _context.SaveChangesAsync() > 0;
        }
    }

}
