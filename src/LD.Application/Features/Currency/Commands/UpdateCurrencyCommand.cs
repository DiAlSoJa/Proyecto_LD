
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

public class UpdateCurrencyCommand : CurrencyRequest, IRequest<Result<string>>
{

}


public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Currency> _currencyRepository;
    private readonly IMapper _mapper;
    public UpdateCurrencyCommandHandler(IRepository<LD.Domain.Entities.Currency> currencyRepository, IMapper mapper)
    {
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currency = await _currencyRepository.GetByIdAsync(request.CurrencyId.Value);
            if (currency is null)
                return Result<string>.Failure("No existe la moneda", new List<string> { "Hubo un error al obtener la moneda" }, 404);
            _mapper.Map(request, currency);

            var result = await _currencyRepository.UpdateAsync(currency);
            return result ? Result<string>.Success("Moneda actualizada con exito", "") : Result<string>.Failure("Hubo un error al actualizar la moneda", new List<string> { "No se encontro la moneda" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar la moneda", new List<string> { ex.Message });
        }
    }
}

