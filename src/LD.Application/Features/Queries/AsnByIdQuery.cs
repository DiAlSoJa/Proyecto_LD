using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;

namespace LD.Application.Features.Queries
{
    public class AsnByIdQuery : IRequest<Result<AsnDto?>>
    {
        public int AsnId { get; set; }
    }

    public class AsnByIdQueryHandler : IRequestHandler<AsnByIdQuery, Result<AsnDto?>>
    {
        private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> _asnRepository;
        private readonly AutoMapper.IMapper _mapper;

        public AsnByIdQueryHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> asnRepository, AutoMapper.IMapper mapper)
        {
            _asnRepository = asnRepository;
            _mapper = mapper;
        }

        public async Task<Result<AsnDto?>> Handle(AsnByIdQuery request, CancellationToken cancellationToken)
        {
            var asn = await _asnRepository.GetByIdAsync(request.AsnId);
            if (asn is null)
                return Result<AsnDto?>.Failure("No existe el ASN", new System.Collections.Generic.List<string> { "No existe el ASN" }, 404);

            var dto = _mapper.Map<AsnDto>(asn);
            return Result<AsnDto?>.Success(dto, "ASN obtenido correctamente");
        }
    }
}
