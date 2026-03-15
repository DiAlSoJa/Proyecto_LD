

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

namespace LD.Application.Features.Units.Comands;

public class CreateUnitCommand : UnitRequest, IRequest<Result<string>>
{

}


public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Units> _unitRepository;
    private readonly IMapper _mapper;
    public CreateUnitCommandHandler(IRepository<LD.Domain.Entities.Units> unitRepository, IMapper mapper)
    {
        _unitRepository = unitRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Units>(request));
            return result ? Result<string>.Success("Unidad creada con exito", "") : Result<string>.Failure("Hubo un error al crear la Unidad", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear la Unidad", new List<string> { ex.Message });
        }
    }
}



