using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin.Services
{
    public interface IOcrService
    {
        Task<string> ReadTextAsync(byte[] imageBytes);
    }
}
