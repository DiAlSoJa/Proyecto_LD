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

namespace LD.Application.Features.Status.Comands;

public class CreateStatusCommand : StatusRequest, IRequest<Result<string>>
{

}


public class CreateStatusCommandHandler : IRequestHandler<CreateStatusCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Status> _statusRepository;
    private readonly IMapper _mapper;
    public CreateStatusCommandHandler(IRepository<LD.Domain.Entities.Status> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _statusRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Status>(request));
            return result ? Result<string>.Success("Estatus creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Estatus", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Estatus", new List<string> { ex.Message });
        }
    }
}



