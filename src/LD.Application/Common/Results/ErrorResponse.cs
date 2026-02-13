using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Results
{

    public class ErrorResponse
    {
        public int? Code { get; set; }
        public IEnumerable<string>? Details { get; set; }
    }
}
