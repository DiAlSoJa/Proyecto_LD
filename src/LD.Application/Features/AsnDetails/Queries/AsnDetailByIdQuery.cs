using LD.Application.Common.Results;
using LD.Contracts.ASN;
using MediatR;

namespace LD.Application.Features.AsnDetails.Queries
{
    public class AsnDetailByIdQuery : IRequest<Result<AsnDetailDto?>>
    {
        public int AsnDetailId { get; set; }
    }

    public class AsnDetailByIdQueryHandler : IRequestHandler<AsnDetailByIdQuery, Result<AsnDetailDto?>>
    {
        private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnDetail> _repo;
        private readonly AutoMapper.IMapper _mapper;

        public AsnDetailByIdQueryHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.AsnDetail> repo, AutoMapper.IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<AsnDetailDto?>> Handle(AsnDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.AsnDetailId);
            if (entity is null)
                return Result<AsnDetailDto?>.Failure("No existe AsnDetail", new System.Collections.Generic.List<string> { "No existe AsnDetail" }, 404);

            var dto = _mapper.Map<AsnDetailDto>(entity);
            return Result<AsnDetailDto?>.Success(dto, "AsnDetail obtenido correctamente");
        }
    }
}
