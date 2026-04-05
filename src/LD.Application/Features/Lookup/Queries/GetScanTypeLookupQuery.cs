using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Lookup.Queries;

public record GetScanTypeLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;

public class GetScanTypeLookupQueryHandler : IRequestHandler<GetScanTypeLookupQuery, Result<List<DropDownDto>>>
{
    private readonly ILookupRepository<ScanType> _repository;

    public GetScanTypeLookupQueryHandler(ILookupRepository<ScanType> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DropDownDto>>> Handle(GetScanTypeLookupQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetLookup();
        return Result<List<DropDownDto>>.Success(data, "Lookups obtenidos con exito");
    }
}
