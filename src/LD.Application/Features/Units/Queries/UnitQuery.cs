
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Units;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Units.Queries;

public class UnitQuery : IRequest<Result<List<UnitDto>?>>
{

}
public class UnitQueryHandler : IRequestHandler<UnitQuery, Result<List<UnitDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Units> _categoryRepository;
    private readonly IMapper _mapper;
    public UnitQueryHandler(IRepository<LD.Domain.Entities.Units> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<UnitDto>?>> Handle(UnitQuery request, CancellationToken cancellationToken)
    {
        var unit = await _categoryRepository.GetManyAsync();
        var unitDtos = _mapper.Map<List<UnitDto>>(unit);
        return Result<List<UnitDto>?>.Success(unitDtos, "Unidades obtenidas correctamente");
    }
}
