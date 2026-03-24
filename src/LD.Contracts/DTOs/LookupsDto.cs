using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs
{
    public class LookupsDto
    {
        public List<DropDownDto> Warehouses { get; set; } = new();
        public List<DropDownDto> Clients { get; set; } = new();
        public List<DropDownDto> Locations { get; set; } = new();
        public List<DropDownDto> Projects { get; set; } = new();

    }
}
