using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Results
{

    public sealed record ErrorResponse(string Code, string Description)
        {
            public static readonly ErrorResponse None = new("", "");
        }
}
