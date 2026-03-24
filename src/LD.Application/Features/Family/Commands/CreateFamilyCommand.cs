
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

public class CreateFamilyCommand : FamilyRequest, IRequest<Result<string>>
{

}


public class CreateFamilyCommandHandler : IRequestHandler<CreateFamilyCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Family> _categoryRepository;
    private readonly IMapper _mapper;
    public CreateFamilyCommandHandler(IRepository<LD.Domain.Entities.Family> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Family>(request));
            return result ? Result<string>.Success("Familia creada con exito", "") : Result<string>.Failure("Hubo un error al crear la Familia", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear la Familia", new List<string> { ex.Message });
        }
    }
}



