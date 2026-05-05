using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.StandardLabel;
using MediatR;

namespace LD.Application.Features.StandardLabels.Queries;

public class StandardLabelByCodeQuery : IRequest<Result<StandardLabelDto?>>
{
    public string Code { get; set; } = string.Empty;
}

public class StandardLabelByCodeQueryHandler : IRequestHandler<StandardLabelByCodeQuery, Result<StandardLabelDto?>>
{
    private readonly IStandarIdService _standarIdService;

    public StandardLabelByCodeQueryHandler(IStandarIdService standarIdService)
    {
        _standarIdService = standarIdService;
    }

    public async Task<Result<StandardLabelDto?>> Handle(StandardLabelByCodeQuery request, CancellationToken cancellationToken)
    {
        var code = request.Code?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(code))
            return Result<StandardLabelDto?>.Failure("Etiqueta LD es obligatoria.", new() { "Etiqueta LD es obligatoria." }, 400);

        var label = await _standarIdService.GetByStandarIdStrAsync(code);
        if (label == null)
            return Result<StandardLabelDto?>.Failure("No existe la etiqueta LD.", new() { "No existe la etiqueta LD." }, 404);

        var isAssigned = await _standarIdService.IsStandarIdAssignedAsync(label.StandarId);

        return Result<StandardLabelDto?>.Success(new StandardLabelDto
        {
            StandarId = label.StandarId,
            StandarIdStr = label.StandarIdStr?.Trim() ?? string.Empty,
            PartNumber = label.PartNumber?.Trim() ?? string.Empty,
            ClientId = label.clientId,
            ProjectId = label.projectId,
            IsAssigned = isAssigned
        }, "Etiqueta LD obtenida correctamente.");
    }
}
