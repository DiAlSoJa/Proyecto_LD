using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;
using System.Collections.Generic;

namespace LD.Application.Features.Queries
{
    public class AsnQuery : IRequest<Result<List<AsnDto>?>>
    {
    }

    public class AsnQueryHandler : IRequestHandler<AsnQuery, Result<List<AsnDto>?>>
    {
        private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> _asnRepository;
        private readonly AutoMapper.IMapper _mapper;

        public AsnQueryHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> asnRepository, AutoMapper.IMapper mapper)
        {
            _asnRepository = asnRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<AsnDto>?>> Handle(AsnQuery request, CancellationToken cancellationToken)
        {
            var asns = await _asnRepository.GetManyAsync();
            var dtos = _mapper.Map<List<AsnDto>>(asns);
            return Result<List<AsnDto>?>.Success(dtos, "ASNs obtenidos correctamente");
        }
    }
}
