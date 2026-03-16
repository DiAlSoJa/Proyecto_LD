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
                         opt => opt.MapFrom(src => src.InventoryStatusIdS))
                     .ForMember(dest => dest.Descripcion,
                         opt => opt.MapFrom(src => src.FullName))
                         .ForMember(dest => dest.Disponible,
                    opt => opt.MapFrom(src => src.IsAvailable)
                         );

            CreateMap<InventaryStatusRequest, LD.Domain.Entities.InventaryStatus>()
                .ForMember(dest => dest.InventoryStatusIdS,
                    opt => opt.MapFrom(src => src.InventoryStatusIdS))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FullName));

            CreateMap<LD.Domain.Entities.InventaryStatus, InventaryStatusRequest>();








        }

    }
}

