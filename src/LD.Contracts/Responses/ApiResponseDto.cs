using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Contracts.Responses
{
    public class ApiResponseDto<T>
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;

        public T? Data { get; init; }
        public int Code { get; init; }
        public List<string>? Errors { get; init; }
        public string? ErrorMessage =>
            Errors == null || Errors.Count == 0
                ? null
                : string.Join(Environment.NewLine, Errors);
        public string Message { get; init; } = string.Empty;
    }
}
