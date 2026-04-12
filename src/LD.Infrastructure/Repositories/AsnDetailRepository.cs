using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.ASN;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class AsnDetailRepository : IAsnDetailRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;

        public AsnDetailRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> DeleteAsync(AsnDetailDto modelToDelete)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepository<AsnDetailDto>.CreateAsync(AsnDetailDto newModoe)
        {
            throw new NotImplementedException();
        }

        async Task<List<AsnDetailDto>> IAsnDetailRepository.GetAsnDetailByAsnIdAsync(int asnId)
        {
            return await _context.AsnDetails
           .AsNoTracking()
           .Where(p => p.AsnId == asnId)
           .ProjectTo<AsnDetailDto>(_mapper.ConfigurationProvider)
           .ToListAsync();
        }

        Task<AsnDetailDto?> IRepository<AsnDetailDto>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<AsnDetailDto?> IRepository<AsnDetailDto>.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<List<AsnDetailDto>?> IRepository<AsnDetailDto>.GetManyAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepository<AsnDetailDto>.UpdateAsync(AsnDetailDto modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
