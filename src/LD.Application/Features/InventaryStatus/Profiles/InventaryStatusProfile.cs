using AutoMapper;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;

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
                .ForMember(dest => dest.ClientId,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.ProjectId,
                    opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Disponible,
                    opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<InventaryStatusRequest, LD.Domain.Entities.InventaryStatus>()
                .ForMember(dest => dest.InventoryStatusIdS,
                    opt => opt.MapFrom(src => src.InventoryStatusIdS))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ClientId,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.ProjectId,
                    opt => opt.MapFrom(src => src.ProjectId));

            CreateMap<LD.Domain.Entities.InventaryStatus, InventaryStatusRequest>()
                .ForMember(dest => dest.ClientId,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.ProjectId,
                    opt => opt.MapFrom(src => src.ProjectId));
        }
    }
}
