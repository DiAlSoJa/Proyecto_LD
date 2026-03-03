using AutoMapper;
using LD.Contracts.DTOs.User;
using LD.Contracts.Item;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Roles.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleDto>();

            CreateMap<RoleRequest, IdentityRole>()
                 .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.RoleName));

        }
    }
}
