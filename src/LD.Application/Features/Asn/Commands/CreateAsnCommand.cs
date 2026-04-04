using System;
using System.Threading;
using System.Threading.Tasks;
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

    private readonly IRepository<LD.Domain.Entities.Asn> _asnRepository;
    private readonly IMapper _mapper;

    public CreateAsnCommandHandler(IRepository<LD.Domain.Entities.Asn> asnRepository, AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAsnCommand request, CancellationToken cancellationToken)
    {
        /*try
        {
            var entity = _mapper.Map<LD.Domain.Entities.Asn>(request);
            var result = await _asnRepository.CreateAsync(entity);
            return result ? Result<string>.Success("ASN creado con exito", "") : Result<string>.Failure("Hubo un error al crear el ASN", new());
        }*/
         try
        {
            var result = await _asnRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Asn>(request));
            return result ? Result<string>.Success("Asn creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Asn", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
