using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;
using System.Collections.Generic;

namespace LD.Application.Features.AsnDetails.Queries
{
    public class AsnDetailQuery : IRequest<Result<List<AsnDetailDto>?>>
    {
        public int AsnId { get; set; }
    }

    public class AsnDetailQueryHandler : IRequestHandler<AsnDetailQuery, Result<List<AsnDetailDto>?>>
    {
        private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnDetail> _repo;
        private readonly AutoMapper.IMapper _mapper;

        public AsnDetailQueryHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnDetail> repo, AutoMapper.IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<List<AsnDetailDto>?>> Handle(AsnDetailQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetManyAsync();
            var dtos = _mapper.Map<List<AsnDetailDto>>(list);
            return Result<List<AsnDetailDto>?>.Success(dtos, "AsnDetails obtenidos correctamente");
        }
    }
}
