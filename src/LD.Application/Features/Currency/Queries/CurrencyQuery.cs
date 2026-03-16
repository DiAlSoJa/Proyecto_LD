using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Currency;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Currency.Queries;

public class CurrencyQuery : IRequest<Result<List<CurrencyDto>?>>
{

}
public class CurrencyQueryHandler : IRequestHandler<CurrencyQuery, Result<List<CurrencyDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Currency> _currencyRepository;
    private readonly IMapper _mapper;
    public CurrencyQueryHandler(IRepository<LD.Domain.Entities.Currency> currencyRepository, IMapper mapper)
    {
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CurrencyDto>?>> Handle(CurrencyQuery request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetManyAsync();
        var currencyDtos = _mapper.Map<List<CurrencyDto>>(currency);
        return Result<List<CurrencyDto>?>.Success(currencyDtos, "Monedas obtenidas correctamente");
    }
}
