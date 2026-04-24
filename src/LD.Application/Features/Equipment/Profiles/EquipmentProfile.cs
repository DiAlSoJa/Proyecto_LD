using AutoMapper;
using LD.Contracts.Equipment;
using LD.Contracts.Requests;

namespace LD.Application.Features.Equipment.Profiles;

public class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<LD.Domain.Entities.Equipment, EquipmentRequest>();

        CreateMap<EquipmentRequest, LD.Domain.Entities.Equipment>()
            .ForMember(dest => dest.EquipmentId, opt => opt.Ignore())
            .ForMember(dest => dest.EquipmentType, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
            .ForMember(dest => dest.EquipmentSupplier, opt => opt.Ignore());

        CreateMap<LD.Domain.Entities.Equipment, EquipmentDto>()
            .ForMember(dest => dest.NoEquipo, opt => opt.MapFrom(src => src.EquipmentName))
            .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.SerialNumber))
            .ForMember(dest => dest.Marca, opt => opt.MapFrom(src => src.Brand ?? string.Empty))
            .ForMember(dest => dest.Horometro, opt => opt.MapFrom(src => src.Hourmeter))
            .ForMember(dest => dest.Operativo, opt => opt.MapFrom(src => src.IsOperative ? "Si" : "No"))
            .ForMember(dest => dest.Turno1, opt => opt.MapFrom(src => src.Turn1))
            .ForMember(dest => dest.Turno2, opt => opt.MapFrom(src => src.Turn2))
            .ForMember(dest => dest.Turno3, opt => opt.MapFrom(src => src.Turn3))
            .ForMember(dest => dest.ImagePathLeft, opt => opt.MapFrom(src => src.ImagePathLeft))
            .ForMember(dest => dest.ImagePathRight, opt => opt.MapFrom(src => src.ImagePathRight))
            .ForMember(dest => dest.Tipo, opt => opt.Ignore())
            .ForMember(dest => dest.Proveedor, opt => opt.Ignore());
    }
}
