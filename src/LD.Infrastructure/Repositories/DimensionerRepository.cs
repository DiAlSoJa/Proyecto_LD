using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class DimensionerRepository : IDimensionerRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;
        public DimensionerRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> CreateAsync(Dimensioner newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<Dimensioner?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Dimensioner?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Dimensioner
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<Dimensioner>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Dimensioner modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
