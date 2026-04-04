using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;

namespace LD.Application.Features.Asn.Queries
{
    public class AsnDetailByAsnIdQuery : IRequest<Result<List<AsnDetailDto>>>
    {
        public int AsnId { get; set; }
    }

    public class AsnDetailByAsnIdQueryHandler : IRequestHandler<AsnDetailByAsnIdQuery, Result<List<AsnDetailDto>>>
    {
        private readonly IAsnDetailRepository _asnRepository;

        public AsnDetailByAsnIdQueryHandler(IAsnDetailRepository asnRepository)
        {
            _asnRepository = asnRepository;
        }

        public async Task<Result<List<AsnDetailDto>>> Handle(AsnDetailByAsnIdQuery request, CancellationToken cancellationToken)
        {
            var asnDetails = await _asnRepository.GetAsnDetailByAsnIdAsync(request.AsnId);

            if (asnDetails is null || !asnDetails.Any())
            {
                return Result<List<AsnDetailDto>>.Failure(
                    "No existen detalles para el ASN",
                    new List<string> { "No existen detalles para el ASN" },
                    404);
            }

            return Result<List<AsnDetailDto>>.Success(asnDetails, "ASN Detalles obtenidos correctamente");
        }
    }
}