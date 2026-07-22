using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Guards;
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
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<LD.Domain.Entities.Asn> _asnParentRepository;

    public DeleteAsnReceiptDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnReceiptDetail> asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnParentRepository = asnParentRepository;
    }

    public async Task<Result<string>> Handle(DeleteAsnReceiptDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validation = await AsnModificationGuard.EnsureAsnReceiptParentIsEditableAsync(
                request.AsnReceiptDetailId,
                _asnRepository,
                _asnDetailRepository,
                _asnParentRepository);
            if (validation is not null)
                return validation;

            var deleteValidation = await AsnModificationGuard.EnsureAsnReceiptIsNotLastForDetailAsync(
                request.AsnReceiptDetailId,
                _asnRepository);
            if (deleteValidation is not null)
                return deleteValidation;

            var asnReceipt = await _asnRepository.GetByIdAsync(request.AsnReceiptDetailId);
            if (asnReceipt is null)
                return Result<string>.Failure("No existe el detalle de recepción del ASN", new List<string> { "No existe el detalle de recepción del ASN" }, 404);

            var deleted = await _asnRepository.DeleteAsync(asnReceipt);
            if (!deleted)
                return Result<string>.Failure("Error al eliminar", new List<string> { "Hubo un error al eliminar el detalle de recepción del ASN" });

            return Result<string>.Success(request.AsnReceiptDetailId.ToString(), "Detalle de recepción del ASN eliminado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al eliminar el detalle de recepción del ASN", new List<string> { ex.Message });
        }
    }
}
