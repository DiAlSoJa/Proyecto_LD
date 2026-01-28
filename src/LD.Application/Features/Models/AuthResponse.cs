using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Models
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        public static AuthResponse Fail(string message)
            => new() { Success = false, Message = message };

        public static AuthResponse Ok(string token)
            => new() { Success = true, Token = token };
    }
}
