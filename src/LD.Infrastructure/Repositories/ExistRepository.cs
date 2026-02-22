using LD.Application.Common.Interfaces;
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
    public class ExistsRepository<T>
    : IExistsRepository<T> 
    {
        protected readonly LdProyectDbContext _context;


        public ExistsRepository(LdProyectDbContext ldProyectDbContext)
        {
            _context = ldProyectDbContext;
        }

       
        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }
    }

}
