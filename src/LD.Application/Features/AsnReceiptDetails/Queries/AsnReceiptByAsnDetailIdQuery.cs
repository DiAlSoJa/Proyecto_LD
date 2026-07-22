using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;

namespace LD.Application.Features.AsnReceiptDetails.Queries
{
    
    public class AsnReceiptByAsnDetailIdQuery : IRequest<Result<List<AsnReceiptDetailDto>>>
    {
        public int AsnDetailId { get; set; }
    }

    public class AsnReceiptByAsnDetailIdQueryHandler : IRequestHandler<AsnReceiptByAsnDetailIdQuery, Result<List<AsnReceiptDetailDto>>>
    {
        private readonly IAsnReceiptDetailRepository _asnRepository;

        public AsnReceiptByAsnDetailIdQueryHandler(IAsnReceiptDetailRepository asnRepository)
        {
            _asnRepository = asnRepository;
        }

        public async Task<Result<List<AsnReceiptDetailDto>>> Handle(AsnReceiptByAsnDetailIdQuery request, CancellationToken cancellationToken)
        {
            var asnDetails = await _asnRepository.GetAsnReceiptByAsnIdAsync(request.AsnDetailId);

            if (asnDetails is null || !asnDetails.Any())
            {
                return Result<List<AsnReceiptDetailDto>>.Failure(
                    "No existen detalles de recepción del ASN para este ASN",
                    new List<string> { "No existen detalles de recepción del ASN para este ASN" },
                    404);
            }

            return Result<List<AsnReceiptDetailDto>>.Success(asnDetails, "Detalles de recepción del ASN obtenidos correctamente");
        }
    }


}
