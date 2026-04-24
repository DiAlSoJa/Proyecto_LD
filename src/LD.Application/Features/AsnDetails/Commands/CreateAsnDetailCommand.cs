using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Guards;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.AsnDetail.Commands;

public class CreateAsnDetailCommand : AsnDetailRequest, IRequest<Result<string>>
{
}

public class CreateAsnDetailCommandHandler : IRequestHandler<CreateAsnDetailCommand, Result<string>>
{

    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnRepository;
    private readonly IRepository<LD.Domain.Entities.Asn> _asnParentRepository;
    private readonly IMapper _mapper;

    public CreateAsnDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnDetail> asnRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository,
        AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _asnParentRepository = asnParentRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAsnDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validation = await AsnModificationGuard.EnsureAsnIsEditableAsync(request.AsnId, _asnParentRepository);
            if (validation is not null)
                return validation;

            var entity = _mapper.Map<LD.Domain.Entities.AsnDetail>(request);
            var result = await _asnRepository.CreateAsync(entity);
            return result
                ? Result<string>.Success(entity.AsnDetailId.ToString(), "Detalle ASN creado con éxito")
                : Result<string>.Failure("Hubo un error al crear el detalle ASN", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el detalle ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
