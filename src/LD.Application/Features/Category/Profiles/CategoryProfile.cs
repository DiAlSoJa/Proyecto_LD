using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.Category;
using LD.Contracts.Requests;


namespace LD.Application.Features.Category.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<LD.Domain.Entities.Category, CategoryDto>()
                 .ForMember(dest => dest.CategoryId,
                    opt => opt.MapFrom(src => src.CategoryId))
                 .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Clave))
                   .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.Description)
                    );

            CreateMap<CategoryRequest, LD.Domain.Entities.Category>()
                .ForMember(dest => dest.CategoryId,
                    opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.Category, CategoryRequest>();
        }

    }
}

