
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LD.Contracts.Category
{
    public class CategoryDto
    {
        [JsonPropertyName("Categoría")]
        public string Categoria { get; set; }
        [JsonPropertyName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;
    }


}


