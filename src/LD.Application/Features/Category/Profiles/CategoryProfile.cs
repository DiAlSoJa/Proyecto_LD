using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.Category;
using LD.Contracts.Requests;
using LD.Contracts.Units;


namespace LD.Application.Features.Category.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<LD.Domain.Entities.Category, CategoryDto>()
           .ForMember(dest => dest.Categoria,
               opt => opt.MapFrom(src => src.CategoryIdS))
           .ForMember(dest => dest.Descripcion,
               opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Frecuencia,
               opt => opt.MapFrom(src => src.Frecuency))
            .ForMember(dest => dest.Cliente,
               opt => opt.MapFrom(src => src.Client))
               .ForMember(dest => dest.Proyecto,
               opt => opt.MapFrom(src => src.Project)
               );

            CreateMap<CategoryRequest, LD.Domain.Entities.Category>()
                .ForMember(dest => dest.CategoryIdS,
                    opt => opt.MapFrom(src => src.CategoryIdS))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            CreateMap<LD.Domain.Entities.Category, CategoryRequest>();
        }

    }
}

