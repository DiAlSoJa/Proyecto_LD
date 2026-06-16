using AutoMapper;
using LD.Application.Features.WarehouseTasks.Commands;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.WarehouseTasks.Profiles;

public class WarehouseTaskProfile : Profile
{
    public WarehouseTaskProfile()
    {
        CreateMap<WarehouseTaskRequest, WarehouseTask>();
        CreateMap<CreateWarehouseTaskCommand, WarehouseTask>();

        CreateMap<WarehouseTask, WarehouseTaskDto>()
            .ForMember(dest => dest.WarehouseName,
                opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.WarehouseName : null))
            // Cast explícito entre enums gemelos (Domain ↔ Contracts).
            // Si los valores dejan de coincidir el test de mapeo fallará aquí, no en silencio.
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => (WarehouseTaskStatus)(int)src.Status));
    }
}
