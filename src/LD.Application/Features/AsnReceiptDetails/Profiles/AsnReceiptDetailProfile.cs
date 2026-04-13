using AutoMapper;
using LD.Contracts.ASN;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.AsnReceiptDetails.Profiles
{
    public class AsnReceiptDetailProfile : Profile
    {
        public AsnReceiptDetailProfile()
        {
            CreateMap<AsnReceiptDetail, AsnReceiptDetailDto>()
                .ForMember(dest => dest.AsnReceiptDetailId, opt => opt.MapFrom(src => src.AsnReceiptDetailId))
                .ForMember(dest => dest.AsnDetailId, opt => opt.MapFrom(src => src.AsnDetailId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.DeleteRow, opt => opt.MapFrom(src => src.DeleteRow))
                .ForMember(dest => dest.StandardId, opt => opt.MapFrom(src => src.StandardId))
                .ForMember(dest => dest.PartNumber, opt => opt.MapFrom(src => src.PartNumber))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StandardQuantity, opt => opt.MapFrom(src => src.StandardQuantity))
                .ForMember(dest => dest.MaximumQuantity, opt => opt.MapFrom(src => src.MaximumQuantity))
                .ForMember(dest => dest.SD, opt => opt.MapFrom(src => src.SD))
                .ForMember(dest => dest.ReceivedQuantity, opt => opt.MapFrom(src => src.ReceivedQuantity))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.LocationCode))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.LotNumber))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
                .ForMember(dest => dest.PurchaseOrder, opt => opt.MapFrom(src => src.PurchaseOrder))
                .ForMember(dest => dest.CustomsDeclarationNumber, opt => opt.MapFrom(src => src.CustomsDeclarationNumber));

            CreateMap<AsnReceiptRequest, AsnReceiptDetail>()
                .ForMember(dest => dest.AsnReceiptDetailId, opt => opt.Ignore());

            CreateMap<AsnReceiptDetail, AsnReceiptRequest>();
        }
    }
}
