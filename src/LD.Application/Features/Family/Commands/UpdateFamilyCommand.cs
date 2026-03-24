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

namespace LD.Application.Features.Family.Comands;

public class UpdateFamilyCommand : FamilyRequest, IRequest<Result<string>>
{

}


public class UpdateFamilyCommandHandler : IRequestHandler<UpdateFamilyCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Family> _categoryRepository;
    private readonly IMapper _mapper;
    public UpdateFamilyCommandHandler(IRepository<LD.Domain.Entities.Family> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateFamilyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(request.FamilyId);
            if (category is null)
                return Result<string>.Failure("No existe la familia", new List<string> { "Hubo un error al obtener la familia" }, 404);
            _mapper.Map(request, category);

            var result = await _categoryRepository.UpdateAsync(category);
            return result ? Result<string>.Success("Familia actualizada con exito", "") : Result<string>.Failure("Hubo un error al actualizar la familia", new List<string> { "No se encontro la categoría" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar la familia", new List<string> { ex.Message });
        }
    }
}

