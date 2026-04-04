using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.ASN;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class AsnRepository : IAsnRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;
        Task<bool> IRepository<Asn>.CreateAsync(Asn newModoe)
        {
            throw new NotImplementedException();
        }

        async Task<List<Asn>> IAsnRepository.GetAsnByClientAsync(int clientId, int projectId)
        {
            return await _context.Asns
            .AsNoTracking()
            .Where(p => p.ClientId == clientId && p.ProjectId==projectId ) 
            .ProjectTo<Asn>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        

        Task<Asn?> IRepository<Asn>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Asn?> IRepository<Asn>.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<List<Asn>?> IRepository<Asn>.GetManyAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepository<Asn>.UpdateAsync(Asn modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
