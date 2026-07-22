using LD.Contracts.Requests;
using LD.Application.Common.Guards;
using LD.Application.Common.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;

namespace LD.Application.Features.Asn.Commands;

public class UpdateAsnDetailCommand : AsnDetailRequest, IRequest<Result<string>>
{
}

public class UpdateAsnDetailCommandHandler : IRequestHandler<UpdateAsnDetailCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnRepository;
    private readonly IRepository<LD.Domain.Entities.Asn> _asnParentRepository;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateAsnDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnDetail> asnRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository,
        AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _asnParentRepository = asnParentRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateAsnDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validation = await AsnModificationGuard.EnsureAsnDetailParentIsEditableAsync(
                request.AsnDetailId,
                _asnRepository,
                _asnParentRepository);
            if (validation is not null)
                return validation;

            var asnDetail = await _asnRepository.GetByIdAsync(request.AsnDetailId);
            if (asnDetail is null)
                return Result<string>.Failure("No existe el detalle de ASN", new System.Collections.Generic.List<string> { "No existe el detalle de ASN" }, 404);

            _mapper.Map(request, asnDetail);

            var updated = await _asnRepository.UpdateAsync(asnDetail);
            if (!updated)
                return Result<string>.Failure("Error al actualizar", new System.Collections.Generic.List<string> { "Hubo un error al actualizar" });

            return Result<string>.Success(asnDetail.AsnDetailId.ToString(), "Detalle de ASN actualizado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el detalle de ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
