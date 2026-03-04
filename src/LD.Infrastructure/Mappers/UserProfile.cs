using AutoMapper;
using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Rol,
                    opt => opt.MapFrom(src => "Todavia no hay roles"));
        }
    }
}
