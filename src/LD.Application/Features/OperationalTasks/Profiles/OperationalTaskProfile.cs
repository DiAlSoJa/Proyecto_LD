using AutoMapper;
using LD.Application.Features.OperationalTasks.Commands;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Features.OperationalTasks.Profiles;

public class OperationalTaskProfile : Profile
{
    public OperationalTaskProfile()
    {
        CreateMap<OperationalTaskRequest, OperationalTask>();
        CreateMap<CreateOperationalTaskCommand, OperationalTask>();
        CreateMap<OperationalTask, OperationalTaskDto>();
    }
}
