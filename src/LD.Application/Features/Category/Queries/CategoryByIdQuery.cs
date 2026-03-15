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

namespace LD.Application.Features.Category.Queries;

public record CategoryByIdQuery(int categoryId)
    : IRequest<Result<CategoryRequest?>>;


public class CategoryByIdQueryHandler : IRequestHandler<CategoryByIdQuery, Result<CategoryRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Category> _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryByIdQueryHandler(IRepository<LD.Domain.Entities.Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<CategoryRequest?>> Handle(CategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDb = await _categoryRepository.GetByIdAsync(request.categoryId);
        if (categoryDb == null) return Result<CategoryRequest?>.Failure("Categoría no encontrada", new(), 404);
        return Result<CategoryRequest?>.Success(_mapper.Map<CategoryRequest>(categoryDb), "Categoría obtenida con exito");
    }
}
