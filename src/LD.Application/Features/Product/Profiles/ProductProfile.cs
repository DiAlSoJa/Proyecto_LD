using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Contracts.DTOs;
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
             
           .ForMember(dest => dest.Descripcion,
               opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.Categoria,
               opt => opt.MapFrom(src => src.Category.Description))
               .ForMember(dest => dest.Familia,
               opt => opt.MapFrom(src => src.Family.FamilyName))
                .ForMember(dest => dest.TipoDeAlmacenamiento,
                opt => opt.MapFrom(src => src.StorageType.Code))
                .ForMember(dest => dest.UnidadMinima,
                opt => opt.MapFrom(src => src.MinUnitId))
                .ForMember(dest => dest.UnidadMedia,
                opt => opt.MapFrom(src => src.MediumUnitId))
                .ForMember(dest => dest.UnidadMaxima,
                opt => opt.MapFrom(src => src.MaxUnitId))
                .ForMember(dest => dest.PaqueteEstandar,
                opt => opt.MapFrom(src => src.StandardPackage))
                .ForMember(dest => dest.SolicitarNumeroLote,
                opt => opt.MapFrom(src => src.RequestLotNumber))
                .ForMember(dest => dest.SolicitarFechaCaducidad,
                opt => opt.MapFrom(src => src.RequestExpirationDate))
                .ForMember(dest => dest.SolicitarPedimento,
                opt => opt.MapFrom(src => src.RequestDeclarationNumber))
                .ForMember(dest => dest.SolicitarTipoCambio,
                opt => opt.MapFrom(src => src.RequestExchangeRate))
                .ForMember(dest => dest.SolicitarOrdenCompra,
                opt => opt.MapFrom(src => src.RequestPurchaseOrder))
                .ForMember(dest => dest.SolicitarReferencia,
                opt => opt.MapFrom(src => src.RequestReference))
                .ForMember(dest => dest.Alto,
                opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Largo,
                opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.Ancho,
                opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Peso,
                opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Dimension,
                opt => opt.MapFrom(src => src.Dimensioner.Description))



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

            CreateMap<LD.Domain.Entities.Product, ProductAutocompleteDto>()
             .ForMember(dest => dest.ItemId,
                 opt => opt.MapFrom(src => src.ProductId))
             .ForMember(dest => dest.NumeroParte,
                 opt => opt.MapFrom(src => src.PartNumber))
             .ForMember(dest => dest.Descripcion,
                 opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.StandardPackageValue,
                 opt => opt.MapFrom(src => src.StandardPackageValue))
             .ForMember(dest => dest.MaxUnitValue,
                 opt => opt.MapFrom(src => src.MaxUnitValue));


        }

    }
}

