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

namespace LD.Application.Features.Items.Comands;

public class CreateItemCommand :ItemRequest, IRequest<Result <string>>
{

}


public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Result<string>>
{
    private readonly IRepository<Item> _itemRepository;
    private readonly IMapper _mapper;
    public CreateItemCommandHandler(IRepository<Item> itemRepository,IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _itemRepository.CreateAsync(_mapper.Map<Item>(request));
            return result ? Result<string>.Success("Item creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Item", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Item", new List<string> { ex.Message });
        }
       
      
    }
}
