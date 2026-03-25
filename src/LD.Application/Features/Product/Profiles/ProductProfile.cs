using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.DTOs.Family;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Units;


namespace LD.Application.Features.Product.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<LD.Domain.Entities.Product, ProductDto>()
            .ForMember(dest => dest.ItemId,
               opt => opt.MapFrom(src => src.ProductId))
           .ForMember(dest => dest.NumeroParte,
               opt => opt.MapFrom(src => src.PartNumber))
              .ForMember(dest => dest.Cliente,
               opt => opt.MapFrom(src => src.Client.CommercialName))
              .ForMember(dest => dest.Proyecto,
               opt => opt.MapFrom(src => src.Project.ProjectName));

            CreateMap<ProductRequest, LD.Domain.Entities.Product>()
                .ForMember(dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.PartNumber,
                    opt => opt.MapFrom(src => src.PartNumber));

            CreateMap<LD.Domain.Entities.Product, ProductRequest>();
        }

    }
}

