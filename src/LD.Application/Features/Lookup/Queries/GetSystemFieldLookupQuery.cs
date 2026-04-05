using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs;
using MediatR;

namespace LD.Application.Features.Lookup.Queries;

public record GetSystemFieldLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;

public class GetSystemFieldLookupQueryHandler : IRequestHandler<GetSystemFieldLookupQuery, Result<List<DropDownDto>>>
{
    private readonly ISystemFieldRepository _systemFieldRepository;

    public GetSystemFieldLookupQueryHandler(ISystemFieldRepository systemFieldRepository)
    {
        _systemFieldRepository = systemFieldRepository;
    }

    public async Task<Result<List<DropDownDto>>> Handle(GetSystemFieldLookupQuery request, CancellationToken cancellationToken)
    {
        var fields = await _systemFieldRepository.GetLookup();

        return Result<List<DropDownDto>>.Success(fields, "Lookups obtenidos con exito");
    }
}
