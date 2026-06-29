using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;
using System.Collections.Generic;

namespace LD.Application.Features.Asn.Queries
{
    public class AsnQuery : IRequest<Result<List<AsnDto>?>>
    {
    }

    public class AsnQueryHandler : IRequestHandler<AsnQuery, Result<List<AsnDto>?>>
    {
        private readonly IAsnRepository _asnRepository;
        private readonly AutoMapper.IMapper _mapper;

        public AsnQueryHandler(IAsnRepository asnRepository, AutoMapper.IMapper mapper)
        {
            _asnRepository = asnRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<AsnDto>?>> Handle(AsnQuery request, CancellationToken cancellationToken)
        {
            var asns = await _asnRepository.GetAllWithRelationsAsync();
            var dtos = _mapper.Map<List<AsnDto>>(asns)
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.AsnId)
                .ToList();
            return Result<List<AsnDto>?>.Success(dtos, "ASNs obtenidos correctamente");
        }
    }
}
