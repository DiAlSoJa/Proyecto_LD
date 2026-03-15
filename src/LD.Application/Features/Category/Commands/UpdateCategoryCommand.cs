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

public class UpdateCategoryCommand : CategoryRequest, IRequest<Result<string>>
{

}


public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Category> _categoryRepository;
    private readonly IMapper _mapper;
    public UpdateCategoryCommandHandler(IRepository<LD.Domain.Entities.Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
            if (category is null)
                return Result<string>.Failure("No existe la categoría", new List<string> { "Hubo un error al obtener la categoría" }, 404);
            _mapper.Map(request, category);

            var result = await _categoryRepository.UpdateAsync(category);
            return result ? Result<string>.Success("Categoría actualizada con exito", "") : Result<string>.Failure("Hubo un error al actualizar la categoría", new List<string> { "No se encontro la categoría" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al categoría la categoría", new List<string> { ex.Message });
        }
    }
}

