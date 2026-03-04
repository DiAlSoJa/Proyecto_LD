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
        public List<string>? Errors { get; init; }
        public string ErrorMessage => string.Join(Environment.NewLine, Errors??new());
        public string Message { get; init; } = string.Empty;
    }
}
