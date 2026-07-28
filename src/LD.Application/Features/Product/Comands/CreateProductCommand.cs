
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

public class CreateProductCommand : ProductRequest, IRequest<Result<string>>
{

}


public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<string>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var clientId = request.ClientId ?? 0;
            var projectId = request.ProjectId ?? 0;
            var partNumber = request.PartNumber?.Trim();

            if (clientId > 0 && projectId > 0
                && await _productRepository.ExistsByClientProjectAndPartNumberAsync(clientId, projectId, partNumber))
            {
                return Result<string>.Failure("Ya existe un artículo con el mismo cliente, proyecto y número de parte.", new());
            }

            request.PartNumber = partNumber;

            var result = await _productRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Product>(request));
            return result ? Result<string>.Success("Articulo creado correctamente", "") : Result<string>.Failure("Hubo un error al crear el item", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el item", new List<string> { ex.Message });
        }
    }
}



