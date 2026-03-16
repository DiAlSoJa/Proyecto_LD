

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

namespace LD.Application.Features.Currency.Comands;

public class CreateCurrencyCommand : CurrencyRequest, IRequest<Result<string>>
{

}


public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Currency> _currencyRepository;
    private readonly IMapper _mapper;
    public CreateCurrencyCommandHandler(IRepository<LD.Domain.Entities.Currency> currencyRepository, IMapper mapper)
    {
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _currencyRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Currency>(request));
            return result ? Result<string>.Success("Moneda creada con exito", "") : Result<string>.Failure("Hubo un error al crear la Moneda", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear la Moneda", new List<string> { ex.Message });
        }
    }
}



