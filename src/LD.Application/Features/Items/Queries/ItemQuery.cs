using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Item;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Items.Queries;

public class ItemQuery : IRequest<Result<List<ItemDto>?>>
{

}
public class ItemQueryHandler : IRequestHandler<ItemQuery, Result<List<ItemDto>?>>
{

    
    private readonly IRepository<Item> _itemRepository;
    private readonly IMapper _mapper;   
    public ItemQueryHandler(IRepository<Item> itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }
    public async Task<Result<List<ItemDto>?>> Handle(ItemQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetManyAsync();
        var itemsDtos = _mapper.Map<List<ItemDto>>(items);
        return Result<List<ItemDto>?>.Success(itemsDtos, "Items obtenidos correctamente");

    }
}
