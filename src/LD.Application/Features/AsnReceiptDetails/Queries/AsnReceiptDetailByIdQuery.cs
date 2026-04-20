using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;

namespace LD.Application.Features.AsnReceiptDetails.Queries
{
    public class AsnReceiptDetailByIdQuery : IRequest<Result<AsnReceiptDetailDto?>>
    {
        public int AsnReceiptDetailId { get; set; }
    }

    public class AsnReceiptDetailByIdQueryHandler : IRequestHandler<AsnReceiptDetailByIdQuery, Result<AsnReceiptDetailDto?>>
    {
        private readonly LD.Application.Common.Interfaces.Repository.IAsnReceiptDetailRepository _repo;

        public AsnReceiptDetailByIdQueryHandler(LD.Application.Common.Interfaces.Repository.IAsnReceiptDetailRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<AsnReceiptDetailDto?>> Handle(AsnReceiptDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _repo.GetByIdAsync(request.AsnReceiptDetailId);
            if (dto is null)
                return Result<AsnReceiptDetailDto?>.Failure("No existe AsnReceiptDetail", new System.Collections.Generic.List<string> { "No existe AsnReceiptDetail" }, 404);

            return Result<AsnReceiptDetailDto?>.Success(dto, "AsnReceiptDetail obtenido correctamente");
        }
    }
}
