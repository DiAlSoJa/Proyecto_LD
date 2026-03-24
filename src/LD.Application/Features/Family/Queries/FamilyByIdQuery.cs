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

namespace LD.Application.Features.Family.Queries;

public record FamilyByIdQuery(int familyId)
    : IRequest<Result<FamilyRequest?>>;


public class FamilyByIdQueryHandler : IRequestHandler<FamilyByIdQuery, Result<FamilyRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Family> _categoryRepository;
    private readonly IMapper _mapper;
    public FamilyByIdQueryHandler(IRepository<LD.Domain.Entities.Family> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<FamilyRequest?>> Handle(FamilyByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDb = await _categoryRepository.GetByIdAsync(request.familyId);
        if (categoryDb == null) return Result<FamilyRequest?>.Failure("Familia no encontrada", new(), 404);
        return Result<FamilyRequest?>.Success(_mapper.Map<FamilyRequest>(categoryDb), "Familia obtenida con exito");
    }
}
