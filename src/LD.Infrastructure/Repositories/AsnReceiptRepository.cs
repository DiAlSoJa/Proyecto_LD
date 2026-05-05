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
    public class AsnReceiptRepository : IAsnReceiptDetailRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;

        public AsnReceiptRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> CreateAsync(AsnReceiptDetailDto newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(AsnReceiptDetailDto modelToDelete)
        {
            throw new NotImplementedException();
        }

        public Task<List<Asn>> GetAllWithRelationsAsync()
        {
            throw new NotImplementedException();
        }

       async public Task<List<AsnReceiptDetailDto>> GetAsnReceiptByAsnIdAsync(int detailAsnId)
        {

            var entities = await _context.AsnReceiptDetails
                .AsNoTracking()
                .Include(x => x.StandardLabel)
                .Where(p => p.AsnDetailId == detailAsnId)
                .ToListAsync();

            return _mapper.Map<List<AsnReceiptDetailDto>>(entities);

        }

        public async Task<AsnReceiptDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _context.AsnReceiptDetails
                .AsNoTracking()
                .Include(x => x.StandardLabel)
                .Where(x => x.AsnReceiptDetailId == id)
                .FirstOrDefaultAsync();

            return entity is null ? null : _mapper.Map<AsnReceiptDetailDto>(entity);
        }

        public Task<AsnReceiptDetailDto?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        async public Task<List<AsnReceiptDetailDto>?> GetManyAsync()
        {
            var entities = await _context.AsnReceiptDetails
              .AsNoTracking()
               .Include(x => x.StandardLabel)
               .ToListAsync();
            return _mapper.Map<List<AsnReceiptDetailDto>>(entities);       
        }


        public Task<bool> UpdateAsync(AsnReceiptDetailDto modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
    
}
