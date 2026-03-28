using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.Currency;
using LD.Contracts.Dimensioner;
using LD.Contracts.DTOs;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;

namespace LD.Application.Features.Dimensioner.Profiles
{
    public class DimensionerProfile : Profile
    {
        public DimensionerProfile()
        {

            CreateMap<LD.Domain.Entities.Dimensioner, DimensionerDto>()
                   .ForMember(dest => dest.DimensionerId,
                    opt => opt.MapFrom(src => src.DimensionerId))
                   .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description)
                    );

            CreateMap<DimensionerRequest, LD.Domain.Entities.Dimensioner>()
                .ForMember(dest => dest.DimensionerId,
                    opt => opt.MapFrom(src => src.DimensionerId))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            CreateMap<LD.Domain.Entities.Dimensioner, DimensionerRequest>();
            CreateMap<LD.Domain.Entities.Dimensioner, DropDownDto>()
            .ForMember(dest => dest.Key,
                opt => opt.MapFrom(src => src.DimensionerId))
            .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Description));





        }

    }
}