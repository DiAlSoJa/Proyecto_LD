using AutoMapper;
using LD.Application.Features.Clients.Queries;
using LD.Contracts.Client;
using LD.Contracts.Requests.Client;
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

                 .ForMember(dest => dest.RazonSocial,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.BusinessName
                            : "N/A"))
                  .ForMember(dest => dest.Rfc,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.Rfc
                            : "N/A"))
                 .ForMember(dest => dest.DomicilioFiscal,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.FiscalAddress
                            : "N/A"))
                .ForMember(dest => dest.ColoniaFiscal,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.Neightbourhoud
                            : "N/A"))
                 .ForMember(dest => dest.CiudadFiscal,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.City
                            : "N/A"))
                .ForMember(dest => dest.CodigoPostalFiscal,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.ZipCode
                            : "N/A"))
                 .ForMember(dest => dest.EmailFiscal,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.Email
                            : "N/A"))
                .ForMember(dest => dest.TelefonoFiscal,
                    opt => opt.MapFrom(src =>
                        src.ClientFiscalData != null
                            ? src.ClientFiscalData.Phone
                            : "N/A"))
                .ForMember(dest => dest.Activo,
                    opt => opt.MapFrom(src => src.IsActive));


            CreateMap<ClientRequest, Client>()
                .ForMember(dest => dest.ClientId,
                    opt => opt.Ignore());

            CreateMap<ClientFiscalDataRequest, ClientFiscalData>()
                .ForMember(dest => dest.ClientFiscalDataId,
                    opt => opt.Ignore()); ;

            CreateMap<Client, ClientRequest>();
            CreateMap<ClientFiscalData, ClientFiscalDataRequest>();

        }
    }
}
