using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LD.Forms.Services
{
    public class LDPAPIService
    {
        public string Decrypt(string cipherText)
        {
            byte[] data = Convert.FromBase64String(cipherText);

            byte[] decrypted = ProtectedData.Unprotect(
                data,
                null,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decrypted);
        }

        public string Encrypt(string plainText)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);

            byte[] encrypted = ProtectedData.Protect(
                data,
                null,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }
    }
}
