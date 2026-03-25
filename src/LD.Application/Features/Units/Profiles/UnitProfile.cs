
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.DTOs;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Units;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;

namespace LD.Application.Features.Clients.Profiles
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            CreateMap<LD.Domain.Entities.Units, UnitDto>()
            .ForMember(dest => dest.Unidad,
                opt => opt.MapFrom(src => src.UnitIdS))
            .ForMember(dest => dest.Descripcion,
                opt => opt.MapFrom(src => src.Description));

            CreateMap<UnitRequest, LD.Domain.Entities.Units>()
                .ForMember(dest => dest.UnitIdS,
                    opt => opt.MapFrom(src => src.UnitIdS))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            CreateMap<LD.Domain.Entities.Units, UnitRequest>();

            CreateMap<LD.Domain.Entities.Units, DropDownDto>()
             .ForMember(dest => dest.Key,
                 opt => opt.MapFrom(src => src.UnitIdS))
             .ForMember(dest => dest.Value,
                 opt => opt.MapFrom(src => src.Description));
        }

    }
}

