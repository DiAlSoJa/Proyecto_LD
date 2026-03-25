using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Requests;

using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Product.Queries;

public record ProductByIdQuery(int productId)
    : IRequest<Result<ProductRequest?>>;


public class ProductByIdQueryHandler : IRequestHandler<ProductByIdQuery, Result<ProductRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Product> _categoryRepository;
    private readonly IMapper _mapper;
    public ProductByIdQueryHandler(IRepository<LD.Domain.Entities.Product> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductRequest?>> Handle(ProductByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDb = await _categoryRepository.GetByIdAsync(request.productId);
        if (categoryDb == null) return Result<ProductRequest?>.Failure("Item no encontrado", new(), 404);
        return Result<ProductRequest?>.Success(_mapper.Map<ProductRequest>(categoryDb), "Item obtenido con exito");
    }
}
