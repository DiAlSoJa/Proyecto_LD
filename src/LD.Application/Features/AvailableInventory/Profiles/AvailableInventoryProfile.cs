using AutoMapper;
using LD.Contracts.AvailableInventory;

namespace LD.Application.Features.AvailableInventories.Profiles;

public class AvailableInventoryProfile : Profile
{
    public AvailableInventoryProfile()
    {
        CreateMap<LD.Domain.Entities.AvailableInventory, AvailableInventoryDto>()
            .ForMember(dest => dest.Cliente,
                opt => opt.MapFrom(src => src.Client != null ? src.Client.CommercialName : string.Empty))
            .ForMember(dest => dest.Proyecto,
                opt => opt.MapFrom(src => src.Project != null ? src.Project.ProjectName : string.Empty))
            .ForMember(dest => dest.Almacen,
                opt => opt.MapFrom(src => src.Location != null && src.Location.Warehouse != null
                    ? src.Location.Warehouse.WarehouseName
                    : string.Empty))
            .ForMember(dest => dest.Ubicacion,
                opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationName : string.Empty))
            .ForMember(dest => dest.StandardIdStr,
                opt => opt.MapFrom(src => src.StandardLabel != null
                    ? src.StandardLabel.StandarIdStr ?? string.Empty
                    : src.StandardId.HasValue ? src.StandardId.Value.ToString() : string.Empty));
    }
}
