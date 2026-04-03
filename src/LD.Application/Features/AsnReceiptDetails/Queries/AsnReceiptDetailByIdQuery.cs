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
        private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnReceiptDetail> _repo;
        private readonly AutoMapper.IMapper _mapper;

        public AsnReceiptDetailByIdQueryHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnReceiptDetail> repo, AutoMapper.IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<AsnReceiptDetailDto?>> Handle(AsnReceiptDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.AsnReceiptDetailId);
            if (entity is null)
                return Result<AsnReceiptDetailDto?>.Failure("No existe AsnReceiptDetail", new System.Collections.Generic.List<string> { "No existe AsnReceiptDetail" }, 404);

            var dto = _mapper.Map<AsnReceiptDetailDto>(entity);
            return Result<AsnReceiptDetailDto?>.Success(dto, "AsnReceiptDetail obtenido correctamente");
        }
    }
}
