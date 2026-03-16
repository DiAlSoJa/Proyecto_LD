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
using LD.Contracts.InventaryStatus;
using MediatR;

namespace LD.Application.Features.Status.Queries;

public class InventaryStatusQuery : IRequest<Result<List<InventaryStatusDto>?>>
{

}
public class StatusQueryHandler : IRequestHandler<InventaryStatusQuery, Result<List<InventaryStatusDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.InventaryStatus> _statusRepository;
    private readonly IMapper _mapper;
    public StatusQueryHandler(IRepository<LD.Domain.Entities.InventaryStatus> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<InventaryStatusDto>?>> Handle(InventaryStatusQuery request, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetManyAsync();
        var statusDtos = _mapper.Map<List<InventaryStatusDto>>(status);
        return Result<List<InventaryStatusDto>?>.Success(statusDtos, "Estatus obtenidos correctamente");
    }
}
