using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;

namespace LD.Application.Features.Asn.Queries
{
    public class AsnByClientIdQuery : IRequest<Result<AsnDto?>>
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
    }

    public class AsnByClientIdQueryHandler : IRequestHandler<AsnByClientIdQuery, Result<AsnDto?>>
    {
        private readonly IAsnRepository _asnRepository;
        private readonly AutoMapper.IMapper _mapper;

        public AsnByClientIdQueryHandler(IAsnRepository asnRepository, AutoMapper.IMapper mapper)
        {
            _asnRepository = asnRepository;
            _mapper = mapper;
        }

        public async Task<Result<AsnDto?>> Handle(AsnByClientIdQuery request, CancellationToken cancellationToken)
        {
            var asn = await _asnRepository.GetAsnByClientAsync(request.ClientId, request.ProjectId);
            if (asn is null)
                return Result<AsnDto?>.Failure("No existe el ASN", new System.Collections.Generic.List<string> { "No existe el ASN" }, 404);

            var dto = _mapper.Map<AsnDto>(asn);
            return Result<AsnDto?>.Success(dto, "ASN obtenido correctamente");
        }
    }
}
