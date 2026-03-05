using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Queries;

public record GetClientLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;


public class GetClientLookupQueryHandler : IRequestHandler<GetClientLookupQuery, Result<List<DropDownDto>>>
{


    private readonly IClientRepository _clientRepository;

    public GetClientLookupQueryHandler(IClientRepository clientRepository)
    {

        _clientRepository = clientRepository;
    }
    public async Task<Result<List<DropDownDto>>> Handle(GetClientLookupQuery request, CancellationToken cancellationToken)
    {
 
        var Clients = await _clientRepository.GetLookup();

        return Result<List<DropDownDto>>.Success(Clients, "Lookups obtenidos con exito");
    }
}
