using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Responses
{
    public class LoginResponse
    {
        public string? Accesstoken { get; set; }
        public string? RefreshToken { get; set; }

    }
}
