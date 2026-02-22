using AutoMapper;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Profiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>();

            CreateMap<WarehouseRequest, Warehouse>();
        }

    }
}
