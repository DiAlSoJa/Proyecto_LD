using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LD.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ErrorResponse error { get; }

        protected Result(bool isSuccess, ErrorResponse _error)
        {
            IsSuccess = isSuccess;
            error = error;
        }

        //public static Result Success() => new(true, error.None)

        public static Result Failure(ErrorResponse error) =>
             new(false, error);
    }
}
