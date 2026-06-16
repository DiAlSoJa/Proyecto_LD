using AutoMapper;
using LD.Application.Features.OperationalTasks.Commands;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.OperationalTasks.Profiles;

public class OperationalTaskProfile : Profile
{
    public OperationalTaskProfile()
    {
        CreateMap<OperationalTaskRequest, OperationalTask>();
        CreateMap<CreateOperationalTaskCommand, OperationalTask>();

        CreateMap<OperationalTask, OperationalTaskDto>()
            .ForMember(dest => dest.WarehouseName,
                opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.WarehouseName : null));
            // Cast explícito entre enums gemelos (Domain ↔ Contracts).
            // Si los valores dejan de coincidir el test de mapeo fallará aquí, no en silencio.
            //.ForMember(dest => dest.Status,
            //    opt => opt.MapFrom(src => (OperationalTaskStatus)(int)src.Status));
    }
}
