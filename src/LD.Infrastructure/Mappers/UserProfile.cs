using AutoMapper;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
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
                    opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault()));

            CreateMap<ApplicationUser, UserRequest>()
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Id).FirstOrDefault()))
                .ForMember(dest => dest.Username,
                    opt => opt.MapFrom(src => src.UserName));

            CreateMap<ApplicationUser, GetUserDto>()
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom(src => new UserDto
                    {
                        Id       = src.Id,
                        Nombre   = src.FullName,
                        UserName = src.UserName,
                        Activo   = src.IsActive,
                        Rol      = src.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault()
                    }))
                .ForMember(dest => dest.Permissions,
                    opt => opt.MapFrom(src => src.UserRoles
                        .SelectMany(ur => ur.Role.RolePermissions)
                        .Select(rp => new PermissionDto
                        {
                            PermissionId   = rp.PermissionId,
                            PermissionName = rp.Permission.PermissionName ?? ""
                        })
                        .ToList()));
        }
    }
}
