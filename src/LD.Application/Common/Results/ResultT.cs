using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Results
{
    public class Result<T> : Result
    {
        public T? Value { get; }

        protected Result(T value)
            : base(true, ErrorResponse.None)
        {
            Value = value;
        }

        protected Result(ErrorResponse error)
            : base(false, error)
        {
            Value = default;
        }

        public static Result<T> Success(T value) =>
            new(value);

        public static Result<T> Failure(ErrorResponse error) =>
            new(error);
    }
}
