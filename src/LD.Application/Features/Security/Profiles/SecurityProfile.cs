using AutoMapper;
using LD.Application.Features.Security.Commands;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.Security.Profiles;

public class SecurityProfile : Profile
{
    public SecurityProfile()
    {
        CreateMap<CreateSecurityRegistrationCommand, SecurityRegistration>()
            .ForMember(dest => dest.LicenciaFoto1, opt => opt.Ignore())
            .ForMember(dest => dest.LicenciaFoto2, opt => opt.Ignore())
            .ForMember(dest => dest.VehiculoFoto1, opt => opt.Ignore())
            .ForMember(dest => dest.VehiculoFoto2, opt => opt.Ignore())
            .ForMember(dest => dest.Firma,         opt => opt.Ignore());

        CreateMap<SecurityRegistration, SecurityRegistrationDto>()
            .ForMember(dest => dest.Estado,        opt => opt.MapFrom(src => (RegistroEstado_e)(int)src.Estado))
            .ForMember(dest => dest.CortinaNumero, opt => opt.MapFrom(src => src.Cortina != null ? src.Cortina.Numero : null));

        CreateMap<Cortina, CortinaDto>();

        CreateMap<SecurityTask, SecurityTaskDto>()
            .ForMember(dest => dest.Placa,        opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.Placa : ""))
            .ForMember(dest => dest.Nombre,       opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.Nombre : ""))
            .ForMember(dest => dest.TipoVehiculo, opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.TipoVehiculo : ""))
            .ForMember(dest => dest.Linea,        opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.Linea : ""))
            .ForMember(dest => dest.CortinaNumero,opt => opt.MapFrom(src => src.SecurityRegistration != null && src.SecurityRegistration.Cortina != null
                                                                                ? src.SecurityRegistration.Cortina.Numero : null));
    }
}
