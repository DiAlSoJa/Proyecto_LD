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
        public List<string>? Errors { get; set; }
        public string Message { get; set; } = string.Empty;

        private Result(bool isSuccess, string message,T? data, int code, List<string>? errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Code = code;
            Errors = errors;
            Message = message;
        }

        // ✅ SUCCESS
        public static Result<T> Success(T data, string message,int code = 200)
            => new(true, message, data, code, null);

        // ✅ FAILURE
        public static Result<T> Failure(string message, List<string>? errors, int code = 400)
            => new(false,message ,default, code, errors);
    }

}
