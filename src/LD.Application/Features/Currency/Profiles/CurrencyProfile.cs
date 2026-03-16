using AutoMapper;
using LD.Contracts.Currency;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Currency.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<LD.Domain.Entities.Currency, CurrencyDto>()
                 .ForMember(dest => dest.CurrencyId,
                    opt => opt.MapFrom(src => src.CurrencyId))
                 .ForMember(dest => dest.Moneda,
                    opt => opt.MapFrom(src => src.Clave))
                   .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.Description)
                    );

            CreateMap<CurrencyRequest, LD.Domain.Entities.Currency>()
                .ForMember(dest => dest.CurrencyId,
                    opt => opt.Ignore());

            CreateMap<LD.Domain.Entities.Currency, CurrencyRequest>();
        }

    }
}