using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Guards;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
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
    private readonly IMapper _mapper;

    public CreateAsnReceiptDetailCommandHandler(
        IRepository<LD.Domain.Entities.AsnReceiptDetail> asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<LD.Domain.Entities.Asn> asnParentRepository,
        AutoMapper.IMapper mapper)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnParentRepository = asnParentRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateAsnReceiptDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validation = await AsnModificationGuard.EnsureAsnDetailParentIsEditableAsync(
                request.AsnDetailId,
                _asnDetailRepository,
                _asnParentRepository);
            if (validation is not null)
                return validation;

            var entity = _mapper.Map<LD.Domain.Entities.AsnReceiptDetail>(request);
            var result = await _asnRepository.CreateAsync(entity);
            return result ? Result<string>.Success(entity.AsnReceiptDetailId.ToString(), "ASN Receipt creado con exito") : Result<string>.Failure("Hubo un error al crear el ASN Receipt", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el ASN", new System.Collections.Generic.List<string> { ex.Message });
        }
    }
}
