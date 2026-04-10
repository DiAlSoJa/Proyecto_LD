using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Units;
using LD.Contracts.Vehicle;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;

namespace LD.Application.Features.Vehicle.Profiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<LD.Domain.Entities.Vehicle, VehicleDto>()
            .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Placas,
                    opt => opt.MapFrom(src => src.Plates))
                 .ForMember(dest => dest.NumeroVehiculo,
                    opt => opt.MapFrom(src => src.VehicleNumber))
                 .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.Tipo,
                    opt => opt.MapFrom(src => src.Type))
                 .ForMember(dest => dest.Capacidad,
                    opt => opt.MapFrom(src => src.Capacity))
                 .ForMember(dest => dest.Placas,
                    opt => opt.MapFrom(src => src.Plates))
                 .ForMember(dest => dest.Largo,
                    opt => opt.MapFrom(src => src.Long))
                 .ForMember(dest => dest.Ancho,
                    opt => opt.MapFrom(src => src.Wight))
                 .ForMember(dest => dest.Alto,
                    opt => opt.MapFrom(src => src.Height)
                    );

            CreateMap<VechicleRequest, LD.Domain.Entities.Vehicle>()
                .ForMember(dest => dest.Plates,
                    opt => opt.MapFrom(src => src.Plates))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name));

            CreateMap<LD.Domain.Entities.Vehicle, VechicleRequest>();









        }

    }
}
