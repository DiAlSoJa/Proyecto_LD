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

namespace LD.Application.Features.Asn.Queries;

public record AsnByIdQuery(int AsnId)
    : IRequest<Result<AsnRequest?>>;


public class AsnByIdQueryHandler : IRequestHandler<AsnByIdQuery, Result<AsnRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Asn> _asnRepository;
    private readonly IMapper _mapper;
    public AsnByIdQueryHandler(IRepository<LD.Domain.Entities.Asn> asnRepository, IMapper mapper)
    {
        _asnRepository = asnRepository;
        _mapper = mapper;
    }

    public async Task<Result<AsnRequest?>> Handle(AsnByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDb = await _asnRepository.GetByIdAsync(request.AsnId);
        if (categoryDb == null) return Result<AsnRequest?>.Failure("Asn no encontrada", new(), 404);
        return Result<AsnRequest?>.Success(_mapper.Map<AsnRequest>(categoryDb), "Asn obtenida con exito");
    }
}
