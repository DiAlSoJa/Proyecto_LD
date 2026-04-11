using LD.Contracts.Requests;
using LD.Application.Common.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LD.Application.Features.Asn.Commands;

public class UpdateAsnCommand : AsnRequest, IRequest<Result<string>>
{
    public int AsnId { get; set; }
}

public class UpdateAsnCommandHandler : IRequestHandler<UpdateAsnCommand, Result<string>>
{
    private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> _asnRepository;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateAsnCommandHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> asnRepository, AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var asn = await _asnRepository.GetByIdAsync(request.AsnId);
            if (asn is null)
                return Result<string>.Failure("No existe el ASN", new System.Collections.Generic.List<string> { "No existe el ASN" }, 404);
            request.AsnCode= asn.AsnCode;
            request.PreAsnCode= asn.PreAsnCode;
            _mapper.Map(request, asn);

            var updated = await _asnRepository.UpdateAsync(asn);
            if (!updated)
                return Result<string>.Failure("Error al actualizar", new System.Collections.Generic.List<string> { "Hubo un error al actualizar" });

            return Result<string>.Success(asn.AsnId.ToString(), "ASN actualizado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
