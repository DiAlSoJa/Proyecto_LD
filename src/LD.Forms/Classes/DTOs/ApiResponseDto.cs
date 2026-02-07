using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Classes.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string>? Errors{ get; set; }

    }
}
