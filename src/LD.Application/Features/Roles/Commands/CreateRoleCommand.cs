using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Roles.Commands;

public class CreateRoleCommand :RoleRequest, IRequest<Result<string>>
{

}

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, Result<string>>
{

    //private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;
    public CreateRoleCommandHandler(IMapper mapper)
    {
        //_roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
 
        //var response = await _roleManager.CreateAsync(_mapper.Map<ApplicationRole>(request));
        
        //if (response.Succeeded)
        //{
        //    return Result<string>.Success("Todo bien","Role creado con exito");
        //}
        return Result<string>.Failure("No se pudo crear el role", new());
    }
}