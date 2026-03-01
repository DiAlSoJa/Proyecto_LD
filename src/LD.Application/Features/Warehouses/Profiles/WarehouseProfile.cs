using AutoMapper;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Profiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>()
                 .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.NombreAlmacen,
                    opt => opt.MapFrom(src => src.WarehouseName))
                .ForMember(dest => dest.Domicilio,
                    opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Colonia,
                    opt => opt.MapFrom(src => src.Neighborhood))
                .ForMember(dest => dest.Ciudad,
                    opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.CodigoPostal,
                    opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive));

            CreateMap<WarehouseRequest, Warehouse>()
                .ForMember(dest => dest.WarehouseId,
                    opt => opt.Ignore());

            CreateMap<Warehouse, WarehouseRequest>();
        }

    }
}
