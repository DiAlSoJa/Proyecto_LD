using AutoMapper;
using LD.Contracts.DTOs;
using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.EquipmentType.Profiles
{
    public class EquipmentTypeProfile : Profile
    {
        public EquipmentTypeProfile()
        {
            CreateMap<LD.Domain.Entities.EquipmentType, EquipmentTypeDto>();

            CreateMap<EquipmentTypeRequest, LD.Domain.Entities.EquipmentType>()
                .ForMember(dest => dest.EquipmentTypeId,
                    opt => opt.MapFrom(src => src.EquipmentTypeId))
                .ForMember(dest => dest.EquipmentName,
                    opt => opt.MapFrom(src => src.EquipmentName))
                .ForMember(dest => dest.IsBattery,
                    opt => opt.MapFrom(src => src.IsBattery))
                .ForMember(dest => dest.ImagePathLeft,
                    opt => opt.MapFrom(src => src.ImagePathLeft))
                .ForMember(dest => dest.ImagePathRight,
                    opt => opt.MapFrom(src => src.ImagePathRight));

            CreateMap<LD.Domain.Entities.EquipmentType, EquipmentTypeRequest>();

            CreateMap<LD.Domain.Entities.EquipmentType, DropDownDto>()
                .ForMember(dest => dest.Key,
                    opt => opt.MapFrom(src => src.EquipmentTypeId))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom(src => src.EquipmentName));
        }
    }
}
