using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public record DeleteCortinaCommand(int CortinaId) : IRequest<Result<string>>;

public class DeleteCortinaCommandHandler : IRequestHandler<DeleteCortinaCommand, Result<string>>
{
    private readonly IRepository<Cortina> _cortinaRepository;

    public DeleteCortinaCommandHandler(IRepository<Cortina> cortinaRepository)
    {
        _cortinaRepository = cortinaRepository;
    }

    public async Task<Result<string>> Handle(DeleteCortinaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cortina = await _cortinaRepository.GetByIdAsync(request.CortinaId);
            if (cortina is null)
                return Result<string>.Failure("No existe la cortina", new List<string> { "Hubo un error al obtener la cortina" }, 404);

            var result = await _cortinaRepository.DeleteAsync(cortina);
            return result
                ? Result<string>.Success("Cortina eliminada con exito", "")
                : Result<string>.Failure("Hubo un error al eliminar la cortina", new List<string> { "No se pudo eliminar la cortina" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al eliminar la cortina", new List<string> { ex.Message });
        }
    }
}
