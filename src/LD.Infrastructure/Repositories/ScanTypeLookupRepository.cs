using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class ScanTypeLookupRepository : ILookupRepository<ScanType>
    {
        private readonly LdProyectDbContext _context;
        private readonly IMapper _mapper;

        public ScanTypeLookupRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.ScanTypes
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
