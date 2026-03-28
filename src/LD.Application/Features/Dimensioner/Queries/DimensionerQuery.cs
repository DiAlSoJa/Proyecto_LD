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
using LD.Contracts.Category;
using LD.Contracts.Dimensioner;
using LD.Contracts.DTOs.Family;
using MediatR;

namespace LD.Application.Features.Dimensioner.Queries;

public class DimensionerQuery : IRequest<Result<List<DimensionerDto>?>>
{

}
public class DimensionerQueryHandler : IRequestHandler<DimensionerQuery, Result<List<DimensionerDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Dimensioner> _dimensionerRepository;
    private readonly IMapper _mapper;
    public DimensionerQueryHandler(IRepository<LD.Domain.Entities.Dimensioner>  dimensionerRepository, IMapper mapper)
    {
        _dimensionerRepository = dimensionerRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<DimensionerDto>?>> Handle(DimensionerQuery request, CancellationToken cancellationToken)
    {
        var category = await _dimensionerRepository.GetManyAsync();
        var categoryDtos = _mapper.Map<List<DimensionerDto>>(category);
        return Result<List<DimensionerDto>?>.Success(categoryDtos, "Dimensiones obtenidas correctamente");
    }
}
