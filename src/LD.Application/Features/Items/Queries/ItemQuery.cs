using LD.Application.Common.Interfaces;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Items.Queries;

public class ItemQuery : IRequest<List<Item>?>
{

}
public class ItemQueryHandler : IRequestHandler<ItemQuery, List<Item>?>
{

    
    private readonly IRepository<Item> _itemRepository;
    public ItemQueryHandler(IRepository<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public async Task<List<Item>?> Handle(ItemQuery request, CancellationToken cancellationToken)
    {

        return await _itemRepository.GetManyAsync();
    }
}
