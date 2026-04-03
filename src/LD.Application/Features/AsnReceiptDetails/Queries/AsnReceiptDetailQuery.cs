using LD.Application.Common.Results;
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
        private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnReceiptDetail> _repo;
        private readonly AutoMapper.IMapper _mapper;

        public AsnReceiptDetailQueryHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnReceiptDetail> repo, AutoMapper.IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<List<AsnReceiptDetailDto>?>> Handle(AsnReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetManyAsync();
            var dtos = _mapper.Map<List<AsnReceiptDetailDto>>(list);
            return Result<List<AsnReceiptDetailDto>?>.Success(dtos, "AsnReceiptDetails obtenidos correctamente");
        }
    }
}
