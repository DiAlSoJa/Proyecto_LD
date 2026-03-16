
using AutoMapper;
using LD.Contracts.Units;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Profiles
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            CreateMap<LD.Domain.Entities.Units, UnitDto>()
                .ForMember(dest => dest.id,
                    opt => opt.MapFrom(src => src.UnitId))
                 .ForMember(dest => dest.Unidad,
                    opt => opt.MapFrom(src => src.Clave))
                   .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.Description)
                    );

            CreateMap<UnitRequest, LD.Domain.Entities.Units>()
                .ForMember(dest => dest.UnitId,
                    opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.Units, UnitRequest>();
        }

    }
}

