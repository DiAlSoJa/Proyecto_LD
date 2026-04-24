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
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _productRepository.GetByIdAsync(request.ProductId);
            if (category is null)
                return Result<string>.Failure("No existe el item", new List<string> { "Hubo un error al obtener el item" }, 404);

            var clientId = request.ClientId ?? category.ClientId ?? 0;
            var projectId = request.ProjectId ?? category.ProjectId ?? 0;
            var partNumber = request.PartNumber?.Trim();

            if (clientId > 0 && projectId > 0
                && await _productRepository.ExistsByClientProjectAndPartNumberAsync(clientId, projectId, partNumber, request.ProductId))
            {
                return Result<string>.Failure("Ya existe un artículo con el mismo cliente, proyecto y número de parte.", new());
            }

            request.PartNumber = partNumber;
            _mapper.Map(request, category);

            var result = await _productRepository.UpdateAsync(category);
            return result ? Result<string>.Success("Item actualizado con exito", "") : Result<string>.Failure("Hubo un error al actualizar el item", new List<string> { "No se encontró el item" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el item", new List<string> { ex.Message });
        }
    }
}

