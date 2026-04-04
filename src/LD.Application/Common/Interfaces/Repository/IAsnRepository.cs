using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.DTOs;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IAsnRepository : IRepository<Asn>
    {       
        
        Task<List<Asn>> GetAsnByClientAsync(int clientId, int projectId);
    }
}
