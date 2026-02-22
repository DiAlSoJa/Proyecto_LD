using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Item;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Items.Queries;

public record ItemByIdQuery(int itemId)
    : IRequest<Result<ItemDto?>>;


public class ItemByIdQueryByIdQueryHandler : IRequestHandler<ItemByIdQuery, Result<ItemDto?>>
{

    
    private readonly IRepository<Item> _itemRepository;
    private readonly IMapper _mapper;
    public ItemByIdQueryByIdQueryHandler(IRepository<Item> itemRepository,IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }
    public async Task<Result<ItemDto?>> Handle(ItemByIdQuery request, CancellationToken cancellationToken)
    {
        var itemDb = await _itemRepository.GetByIdAsync(request.itemId);
        if (itemDb == null) return Result<ItemDto?>.Failure("Item no encontrado", new(), 404);
        return Result<ItemDto?>.Success(_mapper.Map<ItemDto>(itemDb), "Item obtenido con exito");
    }
}
