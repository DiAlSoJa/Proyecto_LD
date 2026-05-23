using AutoMapper;
using LD.Application.Features.Security.Commands;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Domain.Enums;

namespace LD.Application.Features.Security.Profiles;

public class SecurityProfile : Profile
{
    public SecurityProfile()
    {
        CreateMap<CreateSecurityRegistrationCommand, SecurityRegistration>()
            .ForMember(dest => dest.Photos, opt => opt.Ignore());

        CreateMap<SecurityRegistrationPhoto, SecurityPhotoDto>()
            .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => (PhotoCategoria_e)(int)src.Categoria))
            .ForMember(dest => dest.Contenido, opt => opt.Ignore());

        CreateMap<SecurityRegistration, SecurityRegistrationDto>()
            .ForMember(dest => dest.Estado,        opt => opt.MapFrom(src => (RegistroEstado_e)(int)src.Estado))
            .ForMember(dest => dest.CortinaNumero, opt => opt.MapFrom(src => src.Cortina != null ? src.Cortina.Numero : null))
            .ForMember(dest => dest.Firma,         opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.Categoria == PhotoCategoria.Firma)))
            .ForMember(dest => dest.Fotos,         opt => opt.MapFrom(src => src.Photos.Where(p => p.Categoria != PhotoCategoria.Firma).ToList()));

        CreateMap<Cortina, CortinaDto>();

        CreateMap<SecurityTask, SecurityTaskDto>()
            .ForMember(dest => dest.Placa,            opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.Placa : ""))
            .ForMember(dest => dest.Nombre,           opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.Nombre : ""))
            .ForMember(dest => dest.TipoVehiculo,     opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.TipoVehiculo : ""))
            .ForMember(dest => dest.Linea,            opt => opt.MapFrom(src => src.SecurityRegistration != null ? src.SecurityRegistration.Linea : ""))
            .ForMember(dest => dest.CortinaNumero,    opt => opt.MapFrom(src => src.SecurityRegistration != null && src.SecurityRegistration.Cortina != null
                                                                                    ? src.SecurityRegistration.Cortina.Numero : null))
            .ForMember(dest => dest.RegistrationStatus, opt => opt.MapFrom(src => src.SecurityRegistration != null
                                                                                    ? (RegistroEstado_e)(int)src.SecurityRegistration.Estado
                                                                                    : RegistroEstado_e.Registrado));
    }
}
