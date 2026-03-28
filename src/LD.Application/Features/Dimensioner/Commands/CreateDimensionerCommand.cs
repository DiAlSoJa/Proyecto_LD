

using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Dimensioner.Comands;

public class CreateDimensionerCommand : DimensionerRequest, IRequest<Result<string>>
{

}


public class CreateDimensionerCommandHandler : IRequestHandler<CreateDimensionerCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Dimensioner> _dimensionerRepository;
    private readonly IMapper _mapper;
    public CreateDimensionerCommandHandler(IRepository<LD.Domain.Entities.Dimensioner> dimensionerRepository, IMapper mapper)
    {
        _dimensionerRepository = dimensionerRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateDimensionerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dimensionerRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Dimensioner>(request));
            return result ? Result<string>.Success("Dimensión creada con exito", "") : Result<string>.Failure("Hubo un error al crear la Dimensión ", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear la Dimensión", new List<string> { ex.Message });
        }
    }
}



