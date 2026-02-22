using AutoMapper;
using LD.Contracts.Item;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Items.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemRequest, Item>();

        }
    }
}
