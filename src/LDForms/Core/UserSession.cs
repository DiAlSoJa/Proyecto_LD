using System;
using System.Collections.Generic;
using System.Text;
using LD.Formx.Core;

namespace LD.Forms.Core
{
    public static class UserSession
    {
        public static string? AccessToken { get; set; }
        public static string? RefreshToken { get; set; }


        public static void LogOut()
        {
            AccessToken = null;
            RefreshToken = null;
            UserData.Clear();
        }
    }
}
