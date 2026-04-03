using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Client.Configuration
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
