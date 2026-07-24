using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Guards;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.AsnDetail.Commands;

public class CreateAsnReceiptDetailCommand : AsnReceiptRequest, IRequest<Result<string>>
{
}

public class CreateAsnReceiptDetailCommandHandler : IRequestHandler<CreateAsnReceiptDetailCommand, Result<string>>
{

    private readonly IRepository<LD.Domain.Entities.AsnReceiptDetail> _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<LD.Domain.Entities.Asn> _asnParentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IStandarIdService _standarIdService;
    private readonly IMapper _mapper;

    public CreateAsnReceiptDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnReceiptDetail> asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository,
        IProjectRepository projectRepository,
        IStandarIdService standarIdService,
        AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnParentRepository = asnParentRepository;
        _projectRepository = projectRepository;
        _standarIdService = standarIdService;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAsnReceiptDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var parentValidation = await AsnModificationGuard.EnsureAsnDetailParentIsEditableAsync(
                request.AsnDetailId,
                _asnDetailRepository,
                _asnParentRepository);
            if (parentValidation is not null)
                return parentValidation;

            var standardIdResult = await ResolveStandardIdAsync(request.StandardId);
            if (standardIdResult.IsFailure)
                return Result<string>.Failure(standardIdResult.Message, standardIdResult.Errors, standardIdResult.Code);

            request.StandardId = standardIdResult.Data?.ToString();

            var validationResult = await AsnReceiptValidationGuard.EnsureReceiptCanBeSavedAsync(
                request,
                _asnRepository,
                _asnDetailRepository,
                _asnParentRepository,
                _projectRepository);
            if (validationResult is not null)
                return validationResult;

            var entity = _mapper.Map<LD.Domain.Entities.AsnReceiptDetail>(request);
            entity.PalletNumber = await ResolveNextPalletNumberAsync(request.AsnDetailId);
            var result = await _asnRepository.CreateAsync(entity);
            return result
                ? Result<string>.Success(entity.AsnReceiptDetailId.ToString(), "Detalle de recepción del ASN creado con éxito")
                : Result<string>.Failure("Hubo un error al crear el detalle de recepción del ASN", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el detalle de recepción del ASN", new System.Collections.Generic.List<string> { ex.Message });
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

    private async Task<int> ResolveNextPalletNumberAsync(int asnDetailId)
    {
        var asnDetail = await _asnDetailRepository.GetByIdAsync(asnDetailId);
        if (asnDetail is null)
            return 1;

        var asnDetailIds = (await _asnDetailRepository.GetManyAsync() ?? new System.Collections.Generic.List<LD.Domain.Entities.AsnDetail>())
            .Where(x => x.AsnId == asnDetail.AsnId)
            .Select(x => x.AsnDetailId)
            .ToHashSet();

        var receipts = await _asnRepository.GetManyAsync() ?? new System.Collections.Generic.List<LD.Domain.Entities.AsnReceiptDetail>();

        var nextPalletNumber = receipts
            .Where(x => asnDetailIds.Contains(x.AsnDetailId))
            .Select(x => x.PalletNumber)
            .DefaultIfEmpty(0)
            .Max() + 1;

        return nextPalletNumber;
    }

}
