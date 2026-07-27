using AutoMapper;
using LD.Contracts.DTOs;
using LD.Contracts.Requests;
using LD.Contracts.TruckType;
using TruckTypeEntity = LD.Domain.Entities.TruckType;

namespace LD.Application.Features.TruckType.Profiles;

public class TruckTypeProfile : Profile
{
    public TruckTypeProfile()
    {
        CreateMap<TruckTypeEntity, TruckTypeDto>();

        CreateMap<TruckTypeRequest, TruckTypeEntity>()
            .ForMember(dest => dest.TruckTypeId, opt => opt.MapFrom(src => src.TruckTypeId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<TruckTypeEntity, TruckTypeRequest>();

        CreateMap<TruckTypeEntity, DropDownDto>()
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.TruckTypeId.ToString()))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));
    }
}
