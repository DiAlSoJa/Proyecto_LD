using AutoMapper;
using LD.Contracts.ASN;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.Kitting.Profiles;

public class KittingDetailProfile : Profile
{
    public KittingDetailProfile()
    {
        CreateMap<KittingDetail, KittingDetailDto>()
            .ForMember(dest => dest.KittingDetailId, opt => opt.MapFrom(src => src.KittingDetailId))
            .ForMember(dest => dest.KittingId, opt => opt.MapFrom(src => src.KittingId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.PartNumber, opt => opt.MapFrom(src => src.PartNumber))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.SD, opt => opt.MapFrom(src => src.SD))
            .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.LotNumber))
            .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
            .ForMember(dest => dest.CustomerReference, opt => opt.MapFrom(src => src.CustomerReference))
            .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
            .ForMember(dest => dest.PurchaseOrder, opt => opt.MapFrom(src => src.PurchaseOrder))
            .ForMember(dest => dest.CustomsDeclarationNumber, opt => opt.MapFrom(src => src.CustomsDeclarationNumber))
            .ForMember(dest => dest.StandardQuantity, opt => opt.MapFrom(src => src.StandardQuantity))
            .ForMember(dest => dest.MaximumQuantity, opt => opt.MapFrom(src => src.MaximumQuantity));

        CreateMap<KittingDetailRequest, KittingDetail>()
            .ForMember(dest => dest.KittingDetailId, opt => opt.Ignore());

        CreateMap<KittingDetail, KittingDetailRequest>();
    }
}
