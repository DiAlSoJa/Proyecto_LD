using LD.Contracts.Requests;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.Commands;

public class CreateAsnCommand : LD.Contracts.Requests.AsnRequest, IRequest<Result<string>>
{
}

public class CreateAsnCommandHandler : IRequestHandler<CreateAsnCommand, Result<string>>
{
    private readonly LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> _asnRepository;
    private readonly AutoMapper.IMapper _mapper;

    public CreateAsnCommandHandler(LD.Application.Common.Interfaces.Repository.IRepository<LD.Domain.Entities.Asn> asnRepository, AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<LD.Domain.Entities.Asn>(request);
            var result = await _asnRepository.CreateAsync(entity);
            return result ? Result<string>.Success("ASN creado con exito", "") : Result<string>.Failure("Hubo un error al crear el ASN", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
