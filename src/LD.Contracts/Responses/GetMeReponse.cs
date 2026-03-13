using LD.Contracts.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Responses
{
    public class GetMeReponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public AuthorizationDto? Authorization { get; set; }
        
    }
}
