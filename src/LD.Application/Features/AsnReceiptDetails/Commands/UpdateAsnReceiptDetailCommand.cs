using LD.Contracts.Requests;
using LD.Application.Common.Guards;
using LD.Application.Common.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.StandarLabel;

namespace LD.Application.Features.Asn.Commands;

public class UpdateAsnReceiptDetailCommand : AsnReceiptRequest, IRequest<Result<string>>
{
    public int AsnId { get; set; }
}

public class UpdateAsnReceiptDetailCommandHandler : IRequestHandler<UpdateAsnReceiptDetailCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.AsnReceiptDetail> _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<LD.Domain.Entities.Asn> _asnParentRepository;
    private readonly IStandarIdService _standarIdService;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateAsnReceiptDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnReceiptDetail> asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository,
        IStandarIdService standarIdService,
        AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnParentRepository = asnParentRepository;
        _standarIdService = standarIdService;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateAsnReceiptDetailCommand request, CancellationToken cancellationToken)
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

            var asn = await _asnRepository.GetByIdAsync(request.AsnReceiptDetailId);
            if (asn is null)
                return Result<string>.Failure("No existe el ASN Receipt", new System.Collections.Generic.List<string> { "No existe el ASN Receipt" }, 404);

            var existingStandardId = asn.StandardId;
            var standardIdResult = await ResolveStandardIdAsync(request.StandardId);
            if (standardIdResult.IsFailure)
                return Result<string>.Failure(standardIdResult.Message, standardIdResult.Errors, standardIdResult.Code);

            var hasStandardIdInRequest = !string.IsNullOrWhiteSpace(request.StandardId);
            request.StandardId = standardIdResult.Data?.ToString();

            _mapper.Map(request, asn);

            if (!hasStandardIdInRequest)
                asn.StandardId = existingStandardId;

            var updated = await _asnRepository.UpdateAsync(asn);
            if (!updated)
                return Result<string>.Failure("Error al actualizar", new System.Collections.Generic.List<string> { "Hubo un error al actualizar" });

            return Result<string>.Success(asn.AsnReceiptDetailId.ToString(), "ASN Detail actualizado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }

    private async Task<Result<int?>> ResolveStandardIdAsync(string? standardId)
    {
        var value = standardId?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            return Result<int?>.Success(null, string.Empty);

        var label = await _standarIdService.GetByStandarIdStrAsync(value);
        if (label != null)
            return Result<int?>.Success(label.StandarId, string.Empty);

        if (int.TryParse(value, out var parsedStandardId))
            return Result<int?>.Success(parsedStandardId, string.Empty);

        return Result<int?>.Failure("No existe la etiqueta LD.", new() { "No existe la etiqueta LD." }, 404);
    }
}
