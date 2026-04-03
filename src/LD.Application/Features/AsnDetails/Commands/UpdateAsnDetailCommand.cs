using LD.Contracts.Requests;
using LD.Application.Common.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;

namespace LD.Application.Features.Asn.Commands;

public class UpdateAsnDetailCommand : AsnDetailRequest, IRequest<Result<string>>
{
    public int AsnId { get; set; }
}

public class UpdateAsnDetailCommandHandler : IRequestHandler<UpdateAsnDetailCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnRepository;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateAsnDetailCommandHandler(IRepository<LD.Domain.Entities.AsnDetail> asnRepository, AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateAsnDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var asn = await _asnRepository.GetByIdAsync(request.AsnId);
            if (asn is null)
                return Result<string>.Failure("No existe el ASN Detail", new System.Collections.Generic.List<string> { "No existe el ASN Detail" }, 404);

            _mapper.Map(request, asn);

            var updated = await _asnRepository.UpdateAsync(asn);
            if (!updated)
                return Result<string>.Failure("Error al actualizar", new System.Collections.Generic.List<string> { "Hubo un error al actualizar" });

            return Result<string>.Success("ASN Detail actualizado", asn.AsnId.ToString());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
