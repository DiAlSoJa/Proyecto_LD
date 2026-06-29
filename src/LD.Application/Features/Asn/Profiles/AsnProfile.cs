using AutoMapper;
using LD.Contracts.ASN;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.Asn.Profiles
{
    public class AsnProfile : Profile
    {
        public AsnProfile()
        {
            CreateMap<LD.Domain.Entities.Asn, AsnDto>()
                .ForMember(dest => dest.AsnId, opt => opt.MapFrom(src => src.AsnId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.AsnCode, opt => opt.MapFrom(src => src.AsnCode))                
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client != null ? src.Client.CommercialName: string.Empty))                
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project != null ? src.Project.ProjectName : string.Empty))                
                .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
                .ForMember(dest => dest.GuideNumber, opt => opt.MapFrom(src => src.GuideNumber))
                .ForMember(dest => dest.Eta, opt => opt.MapFrom(src => src.Eta))
                .ForMember(dest => dest.PackagesQty, opt => opt.MapFrom(src => src.PackagesQty))
                .ForMember(dest => dest.IsReturn, opt => opt.MapFrom(src => src.IsReturn))
                .ForMember(dest => dest.IsCustomerMovementRequired, opt => opt.MapFrom(src => src.IsCustomerMovementRequired))
                .ForMember(dest => dest.TransportLine, opt => opt.MapFrom(src => src.TransportLine))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.DriverName))
                .ForMember(dest => dest.VehiclePlate, opt => opt.MapFrom(src => src.VehiclePlate))
                .ForMember(dest => dest.SealNumber, opt => opt.MapFrom(src => src.SealNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<AsnRequest, LD.Domain.Entities.Asn>()
                .ForMember(dest => dest.AsnId, opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.Asn, AsnRequest>();
        }
    }
}
