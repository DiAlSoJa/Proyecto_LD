using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Classes.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;

        public T? Data { get; init; }
        public int Code { get; init; }
        public ErrorResponse? Error { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
