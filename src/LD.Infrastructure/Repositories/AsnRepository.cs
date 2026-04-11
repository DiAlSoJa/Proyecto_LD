using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class AsnRepository : IAsnRepository
    {
        private readonly LdProyectDbContext _context;
        private readonly IMapper _mapper;

        public AsnRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        async Task<bool> IRepository<Asn>.CreateAsync(Asn newModel)
        {
            return await CreateWithSequenceAsync(newModel);
        }

        public async Task<bool> CreateWithSequenceAsync(Asn entity)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var project = await _context.Set<Project>()
                    .FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

                if (project is null)
                    throw new Exception("No se encontró el proyecto.");

                if (string.IsNullOrWhiteSpace(project.AsnPrefix))
                    throw new Exception("El proyecto no tiene configurado AsnPrefix.");

                var currentNumber = 1;

                if (!string.IsNullOrWhiteSpace(project.AsnNumber?.ToString()))
                {
                    currentNumber = Convert.ToInt32(project.AsnNumber);
                }

                entity.AsnCode = $"{project.AsnPrefix}{currentNumber:D5}";
                entity.PreAsnCode = entity.AsnCode;


                _context.Set<Asn>().Add(entity);

                project.AsnNumber = (currentNumber + 1);

                _context.Set<Project>().Update(project);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return result > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        async Task<List<Asn>> IAsnRepository.GetAsnByClientAsync(int clientId, int projectId)
        {
            return await _context.Asns
                .AsNoTracking()
                .Where(p => p.ClientId == clientId && p.ProjectId == projectId)
                .ProjectTo<Asn>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<List<Asn>> GetAllWithRelationsAsync()
        {
            return await _context.Asns
                .AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Client)
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