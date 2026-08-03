using AutoMapper;
using LD.Contracts.DTOs;
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
                .ForMember(dest => dest.WarehouseId,
                    opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Almacen,
                    opt => opt.MapFrom(src => src.Warehouse.WarehouseName))
                .ForMember(dest => dest.Ubicacion,
                    opt => opt.MapFrom(src => src.LocationName))
                .ForMember(dest => dest.Aisle,
                    opt => opt.MapFrom(src => src.Aisle))
                .ForMember(dest => dest.Dimension,
                    opt => opt.MapFrom(src => $"{src.Height}x{src.Width}x{src.Depth}"))
                .ForMember(dest => dest.EsFiscal,
                    opt => opt.MapFrom(src => src.IsFiscal))
                .ForMember(dest => dest.ControlTemperatura,
                    opt => opt.MapFrom(src => src.HasControlledTemperature))
                .ForMember(dest => dest.EsRack,
                    opt => opt.MapFrom(src => src.IsRack))
                .ForMember(dest => dest.EsGeneral,
                    opt => opt.MapFrom(src => src.IsGeneral))
                .ForMember(dest => dest.EsCuarentena,
                    opt => opt.MapFrom(src => src.IsCuarentena))
                .ForMember(dest => dest.EsEmbarque,
                    opt => opt.MapFrom(src => src.IsEmbarque))
                .ForMember(dest => dest.EsCompartido,
                    opt => opt.MapFrom(src => src.IsCompartido))
                .ForMember(dest => dest.EsReciboYEmbarque,
                    opt => opt.MapFrom(src => src.IsReciboYEmbarque))
                .ForMember(dest => dest.EsDoble,
                    opt => opt.MapFrom(src => src.IsDoble))
                .ForMember(dest => dest.EsSencillo,
                    opt => opt.MapFrom(src => src.IsSencillo))
                .ForMember(dest => dest.EsTienePaso,
                    opt => opt.MapFrom(src => src.HasPaso))
                .ForMember(dest => dest.EsTieneCortina,
                    opt => opt.MapFrom(src => src.HasCortina))
                .ForMember(dest => dest.Ocupado,
                    opt => opt.MapFrom(src => src.Ocupado))
                .ForMember(dest => dest.Placas,
                    opt => opt.MapFrom(src => src.Placas))
                .ForMember(dest => dest.Rack,
                    opt => opt.MapFrom(src => src.Rack))
                  .ForMember(dest => dest.Posicion,
                    opt => opt.MapFrom(src => src.Level))
                    .ForMember(dest => dest.Nivel,
                    opt => opt.MapFrom(src => src.Position));

            CreateMap<LocationRequest, Location>()
                .ForMember(dest => dest.LocationId,
                    opt => opt.Ignore());

            CreateMap<Location, LocationRequest>();

            CreateMap<Location, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.LocationName));


        }
    }
}
