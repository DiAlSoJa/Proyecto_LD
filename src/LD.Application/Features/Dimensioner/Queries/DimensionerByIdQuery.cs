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

namespace LD.Application.Features.Dimensioner.Queries;

public record DimensionerByIdQuery(string dimensionerId)
    : IRequest<Result<DimensionerRequest?>>;


public class DimensionerByIdQueryHandler : IRequestHandler<DimensionerByIdQuery, Result<DimensionerRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Dimensioner> _dimensionerRepository;
    private readonly IMapper _mapper;
    public DimensionerByIdQueryHandler(IRepository<LD.Domain.Entities.Dimensioner> dimensionerRepository, IMapper mapper)
    {
        _dimensionerRepository = dimensionerRepository;
        _mapper = mapper;
    }

    public async Task<Result<DimensionerRequest?>> Handle(DimensionerByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDb = await _dimensionerRepository.GetByIdAsync(request.dimensionerId);
        if (categoryDb == null) return Result<DimensionerRequest?>.Failure("Dimensión no encontrada", new(), 404);
        return Result<DimensionerRequest?>.Success(_mapper.Map<DimensionerRequest>(categoryDb), "Dimensión obtenida con exito");
    }
}
