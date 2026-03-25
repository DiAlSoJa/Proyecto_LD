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

namespace LD.Application.Features.Product.Comands;

public class UpdateProductCommand : ProductRequest, IRequest<Result<string>>
{

}


public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Product> _categoryRepository;
    private readonly IMapper _mapper;
    public UpdateProductCommandHandler(IRepository<LD.Domain.Entities.Product> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(request.ProductId);
            if (category is null)
                return Result<string>.Failure("No existe el item", new List<string> { "Hubo un error al obtener el item" }, 404);
            _mapper.Map(request, category);

            var result = await _categoryRepository.UpdateAsync(category);
            return result ? Result<string>.Success("Item actualizado con exito", "") : Result<string>.Failure("Hubo un error al actualizar el item", new List<string> { "No se encontró el item" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el item", new List<string> { ex.Message });
        }
    }
}

