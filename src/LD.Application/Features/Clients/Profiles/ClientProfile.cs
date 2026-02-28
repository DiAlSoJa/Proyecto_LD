using AutoMapper;
using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.NombreComercial,
                    opt => opt.MapFrom(src => src.CommercialName))
                .ForMember(dest => dest.DomicilioComercial,
                    opt => opt.MapFrom(src => src.CommercialAddress))
                .ForMember(dest => dest.Ciudad,
                    opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.CodigoPostal,
                    opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(dest => dest.Telefono,
                    opt => opt.MapFrom(src => src.Phone))
                // ⚠️ No existen en entity
                .ForMember(dest => dest.RazonSocial,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Rfc,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive));


            CreateMap<ClientRequest, Client>();
        }
    }
}
