using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.AsnDetails.Commands;

public class DeleteAsnDetailCommand : IRequest<Result<string>>
{
    public int AsnDetailId { get; set; }
}

public class DeleteAsnDetailCommandHandler : IRequestHandler<DeleteAsnDetailCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnRepository;

    public DeleteAsnDetailCommandHandler(IRepository<LD.Domain.Entities.AsnDetail> asnRepository)
    {
        _asnRepository = asnRepository;
    }

    public async Task<Result<string>> Handle(DeleteAsnDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var asnDetail = await _asnRepository.GetByIdAsync(request.AsnDetailId);
            if (asnDetail is null)
                return Result<string>.Failure("No existe el ASN Detail", new List<string> { "No existe el ASN Detail" }, 404);

            var deleted = await _asnRepository.DeleteAsync(asnDetail);
            if (!deleted)
                return Result<string>.Failure("Error al eliminar", new List<string> { "Hubo un error al eliminar el ASN Detail" });

            return Result<string>.Success(request.AsnDetailId.ToString(), "ASN Detail eliminado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al eliminar el ASN Detail", new List<string> { ex.Message });
        }
    }
}
