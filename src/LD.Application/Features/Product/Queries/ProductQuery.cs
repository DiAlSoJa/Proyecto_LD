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
using LD.Contracts.Product;
using MediatR;

namespace LD.Application.Features.Product.Queries;

public class ProductQuery : IRequest<Result<List<ProductDto>?>>
{

}
public class ProductQueryHandler : IRequestHandler<ProductQuery, Result<List<ProductDto>?>>
{
    private readonly IProductRepository _categoryRepository;
    private readonly IMapper _mapper;
    public ProductQueryHandler(IProductRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductDto>?>> Handle(ProductQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAllWithRelationsAsync();
        var categoryDtos = _mapper.Map<List<ProductDto>>(category);
        return Result<List<ProductDto>?>.Success(categoryDtos, "Items obtenidos correctamente");
    }
}
