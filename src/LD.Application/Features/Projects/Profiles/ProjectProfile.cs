using AutoMapper;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.ProjectId,
                    opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Cliente,
                    opt => opt.MapFrom(src => src.Client.CommercialName))
                .ForMember(dest => dest.ClienteId,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Proyecto,
                    opt => opt.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.AlmacenId,
                    opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.Almacen,
                    opt => opt.MapFrom(src => src.Warehouse.WarehouseName))
                ;
            CreateMap<Project, ProjectRequest>();
            CreateMap<ProjectRequest, Project>();
        }
    }
}
