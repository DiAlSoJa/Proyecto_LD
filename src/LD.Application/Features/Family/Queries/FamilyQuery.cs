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
using LD.Contracts.DTOs.Family;
using MediatR;

namespace LD.Application.Features.Family.Queries;

public class FamilyQuery : IRequest<Result<List<FamilyDto>?>>
{

}
public class FamilyQueryHandler : IRequestHandler<FamilyQuery, Result<List<FamilyDto>?>>
{
    private readonly IFamilyRepository _categoryRepository;
    private readonly IMapper _mapper;
    public FamilyQueryHandler(IFamilyRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<FamilyDto>?>> Handle(FamilyQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAllWithRelationsAsync();
        var categoryDtos = _mapper.Map<List<FamilyDto>>(category);
        return Result<List<FamilyDto>?>.Success(categoryDtos, "Familias obtenidas correctamente");
    }
}
