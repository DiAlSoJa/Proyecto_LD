using AutoMapper;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.LocationId,
                    opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.AlmacenId,
                    opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.Almacen,
                    opt => opt.MapFrom(src => src.Warehouse.WarehouseName))
                .ForMember(dest => dest.Ubicacion,
                    opt => opt.MapFrom(src => src.LocationName))
                .ForMember(dest => dest.Dimension,
                    opt => opt.MapFrom(src => $"{ src.Height}x{src.Width}x{src.Depth}"))
                .ForMember(dest => dest.General,
                    opt => opt.MapFrom(src => src.IsGeneral))
                .ForMember(dest => dest.Cuarentena,
                    opt => opt.MapFrom(src => src.IsCuarentena))
                .ForMember(dest => dest.Embarque,
                    opt => opt.MapFrom(src => src.IsEmbarque))
                .ForMember(dest => dest.Rack,
                    opt => opt.MapFrom(src => src.IsRack));

            CreateMap<LocationRequest, Location>()
                .ForMember(dest => dest.LocationId,
                    opt => opt.Ignore());

            CreateMap<Location, LocationRequest>();


        }
    }
}
