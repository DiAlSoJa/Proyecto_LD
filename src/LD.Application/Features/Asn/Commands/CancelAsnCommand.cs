using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.Asn.Commands;

public class CancelAsnCommand : IRequest<Result<string>>
{
    public int AsnId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class CancelAsnCommandHandler : IRequestHandler<CancelAsnCommand, Result<string>>
{
    private readonly IAsnRepository _asnRepository;

    public CancelAsnCommandHandler(IAsnRepository asnRepository)
    {
        _asnRepository = asnRepository;
    }

    public async Task<Result<string>> Handle(CancelAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);

            var asn = await _asnRepository.GetByIdAsync(request.AsnId);

            if (asn == null)
                return Result<string>.Failure("ASN no encontrado.", new(), 404);

            if (string.Equals(asn.Status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta confirmado y no se puede cancelar.", new());

            if (string.Equals(asn.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta cancelado.", new());

            asn.Status = "Cancelado";
            asn.LastModifiedAt = DateTime.Now;
            asn.LastModifiedByUserId = request.UserId;

            var updated = await _asnRepository.UpdateAsync(asn);
            if (!updated)
                return Result<string>.Failure("No se pudo cancelar el ASN.", new());

            return Result<string>.Success(asn.AsnId.ToString(), "ASN cancelado correctamente.");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al cancelar el ASN.", new() { ex.Message });
        }
    }
}
