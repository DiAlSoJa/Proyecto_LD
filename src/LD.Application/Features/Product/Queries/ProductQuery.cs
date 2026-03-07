using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Product;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Items.Queries;

public class ProductQuery : IRequest<Result<List<ProductDto>?>>
{

}
public class ItemQueryHandler : IRequestHandler<ProductQuery, Result<List<ProductDto>?>>
{

    
    private readonly IRepository<Product> _itemRepository;
    private readonly IMapper _mapper;   
    public ItemQueryHandler(IRepository<Product> itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }
    public async Task<Result<List<ProductDto>?>> Handle(ProductQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetManyAsync();
        var itemsDtos = _mapper.Map<List<ProductDto>>(items);
        return Result<List<ProductDto>?>.Success(itemsDtos, "Items obtenidos correctamente");

    }
}
