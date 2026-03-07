using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Product;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Items.Queries;

public record ProductByIdQuery(int itemId)
    : IRequest<Result<ProductDto?>>;


public class ItemByIdQueryByIdQueryHandler : IRequestHandler<ProductByIdQuery, Result<ProductDto?>>
{

    
    private readonly IRepository<Product> _itemRepository;
    private readonly IMapper _mapper;
    public ItemByIdQueryByIdQueryHandler(IRepository<Product> itemRepository,IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }
    public async Task<Result<ProductDto?>> Handle(ProductByIdQuery request, CancellationToken cancellationToken)
    {
        var itemDb = await _itemRepository.GetByIdAsync(request.itemId);
        if (itemDb == null) return Result<ProductDto?>.Failure("Item no encontrado", new(), 404);
        return Result<ProductDto?>.Success(_mapper.Map<ProductDto>(itemDb), "Item obtenido con exito");
    }
}
