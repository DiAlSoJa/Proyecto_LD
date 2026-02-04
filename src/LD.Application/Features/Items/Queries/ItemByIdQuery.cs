using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Items.Queries;

public record ItemByIdQuery(int WarehouseId)
    : IRequest<Item?>;


public class ItemByIdQueryByIdQueryHandler : IRequestHandler<ItemByIdQuery, Item?>
{

    
    private readonly IRepository<Item> _itemRepository;
    public ItemByIdQueryByIdQueryHandler(IRepository<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public async Task<Item?> Handle(ItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await _itemRepository.GetByIdAsync(request.WarehouseId);
    }
}
