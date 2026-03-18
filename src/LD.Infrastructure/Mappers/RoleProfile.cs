using AutoMapper;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Mappers
{
    public class RoleProfile :Profile
    {
        public RoleProfile()
        {
            CreateMap<ApplicationRole, RoleRequest>()
                .ForMember(re => re.RoleName,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(re => re.Permissions,
                    opt => opt.MapFrom(src => src.RolePermissions
                        .Select(p => new PermissionDto{
                          PermissionId=  p.PermissionId ,
                          PermissionName = p.Permission.PermissionName??""
                        })
                        .ToList()));

            CreateMap<ApplicationRole, RolePermissionDto>()
            .ForMember(re => re.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(re => re.RoleName,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(re => re.Permissions,
                opt => opt.MapFrom(src => src.RolePermissions
                    .Select(p => new PermissionDto
                    {
                        PermissionId = p.PermissionId,
                        PermissionName = p.Permission.PermissionName ?? ""
                    })
                    .ToList()));
        }
    }
}
