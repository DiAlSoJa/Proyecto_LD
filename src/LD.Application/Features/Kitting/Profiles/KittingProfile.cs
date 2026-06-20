using AutoMapper;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using KittingEntity = LD.Domain.Entities.Kitting;

namespace LD.Application.Features.Kitting.Profiles
{
    public class KittingProfile : Profile
    {
        public KittingProfile()
        {
            CreateMap<KittingEntity, KittingDto>()
                .ForMember(dest => dest.KittingId, opt => opt.MapFrom(src => src.KittingId))
                .ForMember(dest => dest.KittingCode, opt => opt.MapFrom(src => src.KittingCode))
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client != null ? src.Client.CommercialName : string.Empty))
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project != null ? src.Project.ProjectName : string.Empty))
                .ForMember(dest => dest.Warehouse, opt => opt.MapFrom(src => src.Project != null && src.Project.Warehouse != null ? src.Project.Warehouse.WarehouseName : string.Empty))
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
                .ForMember(dest => dest.Contacto, opt => opt.MapFrom(src => src.Contacto))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Colonia, opt => opt.MapFrom(src => src.Colonia))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.CodigoPostal, opt => opt.MapFrom(src => src.CodigoPostal))
                .ForMember(dest => dest.TipoEntrega, opt => opt.MapFrom(src => src.TipoEntrega))
                .ForMember(dest => dest.FechaProgramada, opt => opt.MapFrom(src => src.FechaProgramada))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<KittingRequest, KittingEntity>()
                .ForMember(dest => dest.KittingId, opt => opt.Ignore());

            CreateMap<KittingEntity, KittingRequest>();
        }
    }
}
