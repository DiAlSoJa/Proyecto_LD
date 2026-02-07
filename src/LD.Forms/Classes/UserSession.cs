using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Classes
{
    public static class UserSession
    {
        public static string? AccessToken { get; set; }

        public static void LogOut()
        {
            AccessToken = null;
        }
    }
}
