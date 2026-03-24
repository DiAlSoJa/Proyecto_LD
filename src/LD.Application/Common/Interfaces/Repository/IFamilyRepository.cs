using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IFamilyRepository : IRepository<Family>
    {
        Task<List<Family>> GetAllWithRelationsAsync();
    }
}
