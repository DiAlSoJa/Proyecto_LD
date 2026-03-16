using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.InventaryStatus;

namespace LD.Application.Features.Status.Profiles
{
    public class InventaryStatusProfile : Profile
    {
        public InventaryStatusProfile()
        {
            CreateMap<LD.Domain.Entities.InventaryStatus, InventaryStatusDto>()
                 .ForMember(dest => dest.StatusId,
                    opt => opt.MapFrom(src => src.InventoryStatusId))
                   .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Clave))
                   .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.FullName))
                    .ForMember(dest => dest.Disponible,
                    opt => opt.MapFrom(src => src.IsAvailable)
                    );

            CreateMap<InventaryStatusRequest, LD.Domain.Entities.InventaryStatus>()
                .ForMember(dest => dest.InventoryStatusId,
                    opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.InventaryStatus, InventaryStatusRequest>();
        }

    }
}

