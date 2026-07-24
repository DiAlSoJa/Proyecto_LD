using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Guards;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

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
    private readonly IProjectRepository _projectRepository;
    private readonly IStandarIdService _standarIdService;
    private readonly IMapper _mapper;

    public UpdateAsnReceiptDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnReceiptDetail> asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository,
        IProjectRepository projectRepository,
        IStandarIdService standarIdService,
        IMapper mapper)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnParentRepository = asnParentRepository;
        _projectRepository = projectRepository;
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

            var currentReceipt = await _asnRepository.GetByIdAsync(request.AsnReceiptDetailId);
            if (currentReceipt is null)
                return Result<string>.Failure(
                    "No existe el detalle de recepcion del ASN",
                    new List<string> { "No existe el detalle de recepcion del ASN" },
                    404);

            var targetAsnDetailId = request.AsnDetailId > 0
                ? request.AsnDetailId
                : currentReceipt.AsnDetailId;
            request.AsnDetailId = targetAsnDetailId;

            var existingStandardId = currentReceipt.StandardId;
            var existingPalletNumber = currentReceipt.PalletNumber;

            var standardIdResult = await ResolveStandardIdAsync(request.StandardId);
            if (standardIdResult.IsFailure)
                return Result<string>.Failure(standardIdResult.Message, standardIdResult.Errors, standardIdResult.Code);

            var hasStandardIdInRequest = !string.IsNullOrWhiteSpace(request.StandardId);
            request.StandardId = standardIdResult.Data?.ToString();

            var validationResult = await AsnReceiptValidationGuard.EnsureReceiptCanBeSavedAsync(
                request,
                _asnRepository,
                _asnDetailRepository,
                _asnParentRepository,
                _projectRepository,
                currentReceipt.AsnReceiptDetailId);
            if (validationResult is not null)
                return validationResult;

            _mapper.Map(request, currentReceipt);

            if (!hasStandardIdInRequest)
                currentReceipt.StandardId = existingStandardId;

            currentReceipt.PalletNumber = existingPalletNumber;

            var updated = await _asnRepository.UpdateAsync(currentReceipt);
            if (!updated)
                return Result<string>.Failure(
                    "Error al actualizar",
                    new List<string> { "Hubo un error al actualizar" });

            return Result<string>.Success(currentReceipt.AsnReceiptDetailId.ToString(), "Detalle de recepcion del ASN actualizado");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                "Hubo un error al actualizar el detalle de recepcion del ASN",
                new List<string> { ex.Message });
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
