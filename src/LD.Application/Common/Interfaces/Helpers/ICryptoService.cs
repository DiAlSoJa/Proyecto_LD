using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Helpers
{
    public interface ICryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
