
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

namespace LD.Application.Features.Warehouses.Queries;

public record UnitByIdQuery(int UnitId)
    : IRequest<Result<UnitRequest?>>;


public class UnitByIdQueryHandler : IRequestHandler<UnitByIdQuery, Result<UnitRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Units> _unitRepository;
    private readonly IMapper _mapper;
    public UnitByIdQueryHandler(IRepository<LD.Domain.Entities.Units> unitRepository, IMapper mapper)
    {
        _unitRepository = unitRepository;
        _mapper = mapper;
    }

    public async Task<Result<UnitRequest?>> Handle(UnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unitDb = await _unitRepository.GetByIdAsync(request.UnitId);
        if (unitDb == null) return Result<UnitRequest?>.Failure("Unidad no encontrada", new(), 404);
        return Result<UnitRequest?>.Success(_mapper.Map<UnitRequest>(unitDb), "Unidad obtenida con exito");
    }
}
