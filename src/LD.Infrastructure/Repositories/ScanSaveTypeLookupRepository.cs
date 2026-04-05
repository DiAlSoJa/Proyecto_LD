using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class ScanSaveTypeLookupRepository : ILookupRepository<ScanSaveType>
    {
        private readonly LdProyectDbContext _context;
        private readonly IMapper _mapper;

        public ScanSaveTypeLookupRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.ScanSaveTypes
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
