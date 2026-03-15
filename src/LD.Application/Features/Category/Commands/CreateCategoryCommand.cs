
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Category.Comands;

public class CreateCategoryCommand : CategoryRequest, IRequest<Result<string>>
{

}


public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Category> _categoryRepository;
    private readonly IMapper _mapper;
    public CreateCategoryCommandHandler(IRepository<LD.Domain.Entities.Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Category>(request));
            return result ? Result<string>.Success("Categoría creada con exito", "") : Result<string>.Failure("Hubo un error al crear la Categoría", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear la Categoría", new List<string> { ex.Message });
        }
    }
}



