using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Status;
using MediatR;

namespace LD.Application.Features.Status.Queries;

public class StatusQuery : IRequest<Result<List<StatusDto>?>>
{

}
public class StatusQueryHandler : IRequestHandler<StatusQuery, Result<List<StatusDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Status> _statusRepository;
    private readonly IMapper _mapper;
    public StatusQueryHandler(IRepository<LD.Domain.Entities.Status> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<StatusDto>?>> Handle(StatusQuery request, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetManyAsync();
        var statusDtos = _mapper.Map<List<StatusDto>>(status);
        return Result<List<StatusDto>?>.Success(statusDtos, "Estatus obtenidos correctamente");
    }
}
