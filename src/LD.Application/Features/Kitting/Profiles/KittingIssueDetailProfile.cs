using AutoMapper;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.Kitting.Profiles;

public class KittingIssueDetailProfile : Profile
{
    private static int? ParseNullableStandardId(string? standardId)
    {
        if (string.IsNullOrWhiteSpace(standardId))
            return null;

        return int.TryParse(standardId, out var parsedValue)
            ? parsedValue
            : null;
    }

    public KittingIssueDetailProfile()
    {
        CreateMap<KittingIssueDetail, KittingIssueDetailDto>()
            .ForMember(dest => dest.KittingReceiptDetailId, opt => opt.MapFrom(src => src.KittingReceiptDetailId))
            .ForMember(dest => dest.KittingDetailId, opt => opt.MapFrom(src => src.KittingDetailId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.StandardId, opt => opt.MapFrom(src => src.StandardLabel != null
                ? src.StandardLabel.StandarIdStr
                : src.StandardId.HasValue ? src.StandardId.Value.ToString() : string.Empty))
            .ForMember(dest => dest.StandardIdStr, opt => opt.MapFrom(src => src.StandardLabel != null
                ? src.StandardLabel.StandarIdStr
                : src.StandardId.HasValue ? src.StandardId.Value.ToString() : string.Empty))
            .ForMember(dest => dest.PartNumber, opt => opt.MapFrom(src => src.PartNumber))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StandardQuantity, opt => opt.MapFrom(src => src.StandardQuantity))
            .ForMember(dest => dest.MaximumQuantity, opt => opt.MapFrom(src => src.MaximumQuantity))
            .ForMember(dest => dest.SD, opt => opt.MapFrom(src => src.SD))
            .ForMember(dest => dest.ReceivedQuantity, opt => opt.MapFrom(src => src.ReceivedQuantity))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.Location != null
                ? src.Location.LocationName
                : src.LocationCode ?? string.Empty))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
            .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.LotNumber))
            .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.PurchaseOrder, opt => opt.MapFrom(src => src.PurchaseOrder))
            .ForMember(dest => dest.CustomsDeclarationNumber, opt => opt.MapFrom(src => src.CustomsDeclarationNumber));

        CreateMap<KittingIssueRequest, KittingIssueDetail>()
            .ForMember(dest => dest.KittingReceiptDetailId, opt => opt.Ignore())
            .ForMember(dest => dest.StandardId, opt => opt.MapFrom(src => ParseNullableStandardId(src.StandardId)));

        CreateMap<KittingIssueDetail, KittingIssueRequest>()
            .ForMember(dest => dest.StandardId, opt => opt.MapFrom(src =>
                src.StandardLabel != null
                    ? src.StandardLabel.StandarIdStr
                    : src.StandardId.HasValue ? src.StandardId.Value.ToString() : null));
    }
}
