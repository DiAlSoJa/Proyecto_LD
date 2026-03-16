
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Requests;

using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Currency.Queries;

public record CurrencyByIdQuery(string CurrencyId)
    : IRequest<Result<CurrencyRequest?>>;


public class CurrencyByIdQueryHandler : IRequestHandler<CurrencyByIdQuery, Result<CurrencyRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Currency> _currencyRepository;
    private readonly IMapper _mapper;
    public CurrencyByIdQueryHandler(IRepository<LD.Domain.Entities.Currency> currencyRepository, IMapper mapper)
    {
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<Result<CurrencyRequest?>> Handle(CurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        var currencyDb = await _currencyRepository.GetByIdAsync(request.CurrencyId);
        if (currencyDb == null) return Result<CurrencyRequest?>.Failure("Moneda no encontrada", new(), 404);
        return Result<CurrencyRequest?>.Success(_mapper.Map<CurrencyRequest>(currencyDb), "Moneda obtenida con exito");
    }
}
