using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Requests;

using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Status.Queries;

public record InventaryStatusByIdQuery(string StatusId, int ClientId, int ProjectId)
    : IRequest<Result<InventaryStatusRequest?>>;


public class InventaryStatusByIdQueryHandler : IRequestHandler<InventaryStatusByIdQuery, Result<InventaryStatusRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.InventaryStatus> _statusRepository;
    private readonly IMapper _mapper;
    public InventaryStatusByIdQueryHandler(IRepository<LD.Domain.Entities.InventaryStatus> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<InventaryStatusRequest?>> Handle(InventaryStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var statusDb = (await _statusRepository.GetManyAsync())
            ?.FirstOrDefault(x =>
                string.Equals(x.InventoryStatusIdS?.Trim(), request.StatusId.Trim(), StringComparison.OrdinalIgnoreCase)
                && x.ClientId == request.ClientId
                && x.ProjectId == request.ProjectId);

        if (statusDb == null) return Result<InventaryStatusRequest?>.Failure("Estatus no encontrado", new(), 404);
        return Result<InventaryStatusRequest?>.Success(_mapper.Map<InventaryStatusRequest>(statusDb), "Estatus obtenido con exito");
    }
}
