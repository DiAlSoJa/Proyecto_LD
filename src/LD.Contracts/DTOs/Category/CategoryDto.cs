
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LD.Contracts.Category
{
    public class CategoryDto
    {
        [DisplayName("Id")]
        public int CategoriaId { get; set; }

        [DisplayName("Categoría")]
        public string Categoria { get; set; }
     
        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;
        public int Frecuencia { get; set; } 
        
        public string Cliente { get; set; } = string.Empty;
        public string Proyecto { get; set; } = string.Empty;
    }


}


