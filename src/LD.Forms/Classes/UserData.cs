using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Classes
{
    public static class UserData
    {
        public static string? Id { get; set; }
        public static string? UserName { get; set; }
        public static string? Email { get; set; }

        public static void SetUserData(UserDto user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
        }
        public static void Clear()
        {
            UserName = null;
            Id = null;
            Email = null;
        }
    }
}
