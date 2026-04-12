using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.AsnReceiptDetails.Commands;

public class DeleteAsnReceiptDetailCommand : IRequest<Result<string>>
{
    public int AsnReceiptDetailId { get; set; }
}

public class DeleteAsnReceiptDetailCommandHandler : IRequestHandler<DeleteAsnReceiptDetailCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.AsnReceiptDetail> _asnRepository;

    public DeleteAsnReceiptDetailCommandHandler(IRepository<LD.Domain.Entities.AsnReceiptDetail> asnRepository)
    {
        _asnRepository = asnRepository;
    }

    public async Task<Result<string>> Handle(DeleteAsnReceiptDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var asnReceipt = await _asnRepository.GetByIdAsync(request.AsnReceiptDetailId);
            if (asnReceipt is null)
                return Result<string>.Failure("No existe el ASN Receipt", new List<string> { "No existe el ASN Receipt" }, 404);

            var deleted = await _asnRepository.DeleteAsync(asnReceipt);
            if (!deleted)
                return Result<string>.Failure("Error al eliminar", new List<string> { "Hubo un error al eliminar el ASN Receipt" });

            return Result<string>.Success(request.AsnReceiptDetailId.ToString(), "ASN Receipt eliminado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al eliminar el ASN Receipt", new List<string> { ex.Message });
        }
    }
}
