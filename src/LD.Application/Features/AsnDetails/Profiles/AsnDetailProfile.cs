using AutoMapper;
using LD.Contracts.ASN;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.AsnDetails.Profiles
{
    public class AsnDetailProfile : Profile
    {
        public AsnDetailProfile()
        {
            CreateMap<LD.Domain.Entities.AsnDetail, AsnDetailDto>()
                .ForMember(dest => dest.AsnDetailId, opt => opt.MapFrom(src => src.AsnDetailId))
                .ForMember(dest => dest.AsnId, opt => opt.MapFrom(src => src.AsnId))
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
                .ForMember(dest => dest.Split, opt => opt.MapFrom(src => src.IsSplit));

            CreateMap<AsnDetailRequest, LD.Domain.Entities.AsnDetail>()
                .ForMember(dest => dest.AsnDetailId, opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.AsnDetail, AsnDetailRequest>();
        }
    }
}
