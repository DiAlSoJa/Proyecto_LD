
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

namespace LD.Application.Features.Dimensioner.Comands;

public class UpdateDimensionerCommand : DimensionerRequest, IRequest<Result<string>>
{

}


public class UpdateDimensionerCommandHandler : IRequestHandler<UpdateDimensionerCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Dimensioner> _dimensionerRepository;
    private readonly IMapper _mapper;
    public UpdateDimensionerCommandHandler(IRepository<LD.Domain.Entities.Dimensioner> dimensionerRepository, IMapper mapper)
    {
        _dimensionerRepository = dimensionerRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateDimensionerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currency = await _dimensionerRepository.GetByIdAsync(request.DimensionerId);
            if (currency is null)
                return Result<string>.Failure("No existe la dimensión", new List<string> { "Hubo un error al obtener la dimensión" }, 404);
            _mapper.Map(request, currency);

            var result = await _dimensionerRepository.UpdateAsync(currency);
            return result ? Result<string>.Success("Dimensión actualizada con exito", "") : Result<string>.Failure("Hubo un error al actualizar la dimensión", new List<string> { "No se encontro la dimensión" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar la dimensión", new List<string> { ex.Message });
        }
    }
}

