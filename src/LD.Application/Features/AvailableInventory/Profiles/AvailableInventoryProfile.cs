using AutoMapper;
using LD.Contracts.AvailableInventory;

namespace LD.Application.Features.AvailableInventories.Profiles;

public class AvailableInventoryProfile : Profile
{
    private static string ResolveStandardIdText(int? standardId, string? standardIdStr)
    {
        if (!string.IsNullOrWhiteSpace(standardIdStr))
            return standardIdStr.Trim();

        return standardId.HasValue && standardId.Value > 0
            ? standardId.Value.ToString()
            : string.Empty;
    }

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
            .ForMember(dest => dest.WarehouseId,
                opt => opt.MapFrom(src => src.Location != null
                    ? src.Location.WarehouseId
                    : (int?)null))
            .ForMember(dest => dest.Ubicacion,
                opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationName : string.Empty))
            .ForMember(dest => dest.StandardIdStr,
                opt => opt.MapFrom(src => ResolveStandardIdText(
                    src.StandardId,
                    src.StandardLabel != null ? src.StandardLabel.StandarIdStr : null)));
    }
}
