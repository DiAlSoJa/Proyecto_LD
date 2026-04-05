using AutoMapper;
using LD.Contracts.DTOs;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Domain.Entities;

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
                .ForMember(dest => dest.Proyecto,
                    opt => opt.MapFrom(src => src.ProjectName))                
                .ForMember(dest => dest.Almacen,
                    opt => opt.MapFrom(src => src.Warehouse.WarehouseName))
                ;
            CreateMap<Project, ProjectRequest>();

            CreateMap<ProjectRequest, Project>()
                 .ForMember(dest => dest.ProjectId,
                    opt => opt.Ignore())
                 .ForMember(dest => dest.ScanConfigurations,
                    opt => opt.Ignore());

            CreateMap<ScanConfigurationRequest, ScanConfiguration>()
                .ForMember(dest => dest.ScanConfigurationId, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
                .ForMember(dest => dest.ScanTypeId, opt => opt.MapFrom(src => src.ScanTypeId ?? 0))
                .ForMember(dest => dest.SaveTypeId, opt => opt.MapFrom(src => src.SaveTypeId ?? 0))
                .ForMember(dest => dest.ScanType, opt => opt.Ignore())
                .ForMember(dest => dest.SaveType, opt => opt.Ignore())
                .ForMember(dest => dest.SystemField, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore());

            CreateMap<ScanConfiguration, ScanConfigurationRequest>()
                .ForMember(dest => dest.ScanTypeId, opt => opt.MapFrom(src => src.ScanTypeId))
                .ForMember(dest => dest.SaveTypeId, opt => opt.MapFrom(src => src.SaveTypeId))
                .ForMember(dest => dest.SystemFieldName,
                    opt => opt.MapFrom(src => src.SystemField != null ? src.SystemField.SystemFieldName : ""));

            CreateMap<Project, DropDownDto>()
              .ForMember(dest => dest.Key,
                  opt => opt.MapFrom(src => src.ProjectId))
              .ForMember(dest => dest.Value,
                  opt => opt.MapFrom(src => src.ProjectName));
        }
    }
}
