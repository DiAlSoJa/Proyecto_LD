
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
    private readonly IRepository<LD.Domain.Entities.Product> _categoryRepository;
    private readonly IMapper _mapper;
    public CreateProductCommandHandler(IRepository<LD.Domain.Entities.Product> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Product>(request));
            return result ? Result<string>.Success("Item creado con exito", "") : Result<string>.Failure("Hubo un error al crear el item", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el item", new List<string> { ex.Message });
        }
    }
}



