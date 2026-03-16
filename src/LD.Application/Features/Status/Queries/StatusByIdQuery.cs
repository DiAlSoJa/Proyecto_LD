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

public record StatusByIdQuery(int StatusId)
    : IRequest<Result<StatusRequest?>>;


public class StatusByIdQueryHandler : IRequestHandler<StatusByIdQuery, Result<StatusRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Status> _statusRepository;
    private readonly IMapper _mapper;
    public StatusByIdQueryHandler(IRepository<LD.Domain.Entities.Status> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<StatusRequest?>> Handle(StatusByIdQuery request, CancellationToken cancellationToken)
    {
        var statusDb = await _statusRepository.GetByIdAsync(request.StatusId);
        if (statusDb == null) return Result<StatusRequest?>.Failure("Estatus no encontrado", new(), 404);
        return Result<StatusRequest?>.Success(_mapper.Map<StatusRequest>(statusDb), "Estatus obtenido con exito");
    }
}
