using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.Family;
using LD.Contracts.Requests;
using LD.Contracts.Units;


namespace LD.Application.Features.Category.Profiles
{
    public class FamilyProfile : Profile
    {
        public FamilyProfile()
        {
            CreateMap<LD.Domain.Entities.Family, FamilyDto>()
            .ForMember(dest => dest.FamiliaId,
               opt => opt.MapFrom(src => src.FamilyId))
           .ForMember(dest => dest.NombreFamilia,
               opt => opt.MapFrom(src => src.FamilyName))          
              .ForMember(dest => dest.Cliente,
               opt => opt.MapFrom(src => src.Client.CommercialName))
              .ForMember(dest => dest.Proyecto,
               opt => opt.MapFrom(src => src.Project.ProjectName));

            CreateMap<FamilyRequest, LD.Domain.Entities.Family>()
                .ForMember(dest => dest.FamilyId,
                    opt => opt.MapFrom(src => src.FamilyId))
                .ForMember(dest => dest.FamilyName,
                    opt => opt.MapFrom(src => src.FamilyName));

            CreateMap<LD.Domain.Entities.Family, FamilyRequest>();

            CreateMap<LD.Domain.Entities.Family, DropDownDto>()
             .ForMember(dest => dest.Key,
                 opt => opt.MapFrom(src => src.FamilyId))
             .ForMember(dest => dest.Value,
                 opt => opt.MapFrom(src => src.FamilyName));


        }

    }
}

