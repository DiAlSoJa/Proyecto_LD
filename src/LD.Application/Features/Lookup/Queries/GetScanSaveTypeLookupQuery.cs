using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Lookup.Queries;

public record GetScanSaveTypeLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;

public class GetScanSaveTypeLookupQueryHandler : IRequestHandler<GetScanSaveTypeLookupQuery, Result<List<DropDownDto>>>
{
    private readonly ILookupRepository<ScanSaveType> _repository;

    public GetScanSaveTypeLookupQueryHandler(ILookupRepository<ScanSaveType> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DropDownDto>>> Handle(GetScanSaveTypeLookupQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetLookup();
        return Result<List<DropDownDto>>.Success(data, "Lookups obtenidos con exito");
    }
}
