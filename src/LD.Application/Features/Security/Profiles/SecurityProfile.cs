using AutoMapper;
using LD.Application.Features.Security.Commands;
using LD.Contracts.DTOs.Security;
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

        CreateMap<SecurityRegistration, SecurityRegistrationDto>();
    }
}
