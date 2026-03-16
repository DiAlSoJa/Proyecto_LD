using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Status;

namespace LD.Application.Features.Status.Profiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<LD.Domain.Entities.Status, StatusDto>()
                 .ForMember(dest => dest.StatusId,
                    opt => opt.MapFrom(src => src.StatusId))
                   .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Clave))
                   .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Disponible,
                    opt => opt.MapFrom(src => src.IsAvailable)
                    );

            CreateMap<StatusRequest, LD.Domain.Entities.Status>()
                .ForMember(dest => dest.StatusId,
                    opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.Status, StatusRequest>();
        }

    }
}

