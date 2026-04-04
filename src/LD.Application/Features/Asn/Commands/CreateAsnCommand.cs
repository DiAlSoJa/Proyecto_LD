using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.Asn.Commands;

public class CreateAsnCommand : AsnRequest, IRequest<Result<string>>
{
}

public class CreateAsnCommandHandler : IRequestHandler<CreateAsnCommand, Result<string>>
{
    private readonly IAsnRepository _asnRepository;
    private readonly IMapper _mapper;

    public CreateAsnCommandHandler(IAsnRepository asnRepository, IMapper mapper)
    {
        _asnRepository = asnRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<LD.Domain.Entities.Asn>(request);

            var result = await _asnRepository.CreateWithSequenceAsync(entity);

            return result
                ? Result<string>.Success("Asn creado con éxito", entity.AsnCode ?? string.Empty)
                : Result<string>.Failure("Hubo un error al crear el ASN", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el ASN",
                new List<string> { ex.Message });
        }
    }
}