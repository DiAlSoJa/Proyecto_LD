using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Items.Comands;

public class CreateItemCommand : IRequest<string>
{

}


public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, string>
{
    private readonly IRepository<Item> _itemRepository;
    public CreateItemCommandHandler(IRepository<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<string> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var result = await _itemRepository.CreateAsync(new Item
        {
   
        });
        return result ? "Cliente creado con exito" : "Hubo un error al crear el cliente";
    }
}
