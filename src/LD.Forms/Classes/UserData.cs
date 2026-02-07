using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Classes
{
    public static class UserData
    {
        public static int? Id { get; set; }
        public static string? UserName { get; set; }
        public static string? Email { get; set; }

        public static void Clear()
        {
            UserName = null;
            Id = null;
            Email = null;
        }
    }
}
