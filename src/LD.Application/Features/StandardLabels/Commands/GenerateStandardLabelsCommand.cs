using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.StandardLabels.Commands;

public class GenerateStandardLabelsCommand : GenerateStandardLabelsRequest, IRequest<Result<List<string>>>
{
}

public class GenerateStandardLabelsCommandHandler : IRequestHandler<GenerateStandardLabelsCommand, Result<List<string>>>
{
    private readonly IStandarIdService _standarIdService;

    public GenerateStandardLabelsCommandHandler(IStandarIdService standarIdService)
    {
        _standarIdService = standarIdService;
    }

    public async Task<Result<List<string>>> Handle(GenerateStandardLabelsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Quantity <= 0)
                return Result<List<string>>.Failure("La cantidad de etiquetas debe ser mayor a cero.", new());

            var standardIds = await _standarIdService.GenerateStandarIdsAsync(request.Quantity);
            return Result<List<string>>.Success(standardIds, "StandardId generados correctamente.");
        }
        catch (Exception ex)
        {
            return Result<List<string>>.Failure(ex.Message, new() { ex.Message });
        }
    }
}
