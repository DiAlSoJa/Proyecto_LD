using AutoMapper;
using LD.Contracts.DTOs;
using LD.Domain.Entities;

namespace LD.Infrastructure.Mappers
{
    public class LookupProfile : Profile
    {
        public LookupProfile()
        {
            CreateMap<Client, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.CommercialName));

            CreateMap<Warehouse, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.WarehouseName)); 

            CreateMap<Location, DropDownDto>()
                 .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.LocationName));

            CreateMap<ApplicationRole, DropDownDto>()
                 .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.Name));

            CreateMap<SystemField, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.SystemFieldId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.DisplayName));

            CreateMap<ScanType, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.ScanTypeId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.ScanTypeName));

            CreateMap<ScanSaveType, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.ScanSaveTypeId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.ScanSaveTypeName));
        }
    }
}
