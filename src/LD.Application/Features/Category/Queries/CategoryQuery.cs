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

using MediatR;

namespace LD.Application.Features.Category.Queries;

public class CategoryQuery : IRequest<Result<List<CategoryDto>?>>
{

}
public class CategoryQueryHandler : IRequestHandler<CategoryQuery, Result<List<CategoryDto>?>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryQueryHandler(ICategoryRepository  categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryDto>?>> Handle(CategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAllWithRelationsAsync();
        var categoryDtos = _mapper.Map<List<CategoryDto>>(category);
        return Result<List<CategoryDto>?>.Success(categoryDtos, "Categorías obtenidas correctamente");
    }
}
