using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Views.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
