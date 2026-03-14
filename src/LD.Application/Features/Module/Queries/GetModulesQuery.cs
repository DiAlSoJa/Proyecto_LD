using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.Product;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Module.Queries;

public record GetModulesQuery()
    : IRequest<Result<List<ModuleAuthorizationDto>?>>;


public class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, Result<List<ModuleAuthorizationDto>?>>
{

    private readonly IModuleRepository _moduleRepository;
    public GetModulesQueryHandler(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }
    public async Task<Result<List<ModuleAuthorizationDto>?>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
    {
        var modules = await _moduleRepository.GetModules();
        return Result<List<ModuleAuthorizationDto>?>.Success(modules, "Modulos obtenidos con exito");

    }
}
