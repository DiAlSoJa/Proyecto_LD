using LD.Application.Common.Results;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.ASN;
using MediatR;
using System.Collections.Generic;

namespace LD.Application.Features.AsnReceiptDetails.Queries
{
    public class AsnReceiptDetailQuery : IRequest<Result<List<AsnReceiptDetailDto>?>>
    {
        public int AsnDetailId { get; set; }
    }

    public class AsnReceiptDetailQueryHandler : IRequestHandler<AsnReceiptDetailQuery, Result<List<AsnReceiptDetailDto>?>>
    {
        private readonly IAsnReceiptDetailRepository _repo;

        public AsnReceiptDetailQueryHandler(IAsnReceiptDetailRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<List<AsnReceiptDetailDto>?>> Handle(AsnReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetManyAsync();
            return Result<List<AsnReceiptDetailDto>?>.Success(list, "Detalles de recepción del ASN obtenidos correctamente");
        }
    }
}
