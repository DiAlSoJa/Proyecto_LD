using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Results
{
    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(bool isSuccess, T value, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }
        
        public static Result<T> Success(T value)
            => new Result<T>(true, value, null);

        public static new Result<T> Failure(string error)
            => new Result<T>(false, default, error);
    }
}
