using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T? Data { get; }
        public int Code { get; }
        public ErrorResponse? Error { get; }
        public string Message { get; set; } = string.Empty;

        private Result(bool isSuccess, string message,T? data, int code, ErrorResponse? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Code = code;
            Error = error;
            Message = message;
        }

        // ✅ SUCCESS
        public static Result<T> Success(T data, string message,int code = 200)
            => new(true, message, data, code, null);

        // ✅ FAILURE
        public static Result<T> Failure(string message, ErrorResponse error, int code = 400)
            => new(false,message ,default, code, error);
    }

}
