using LD.Contracts.DTOs;
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
        public static bool? Activo { get; set; }
        public static string? Rol { get; set; }


        public static LookupsDto? Lookups { get; set; }

        public static void SetUserData(UserDto user, LookupsDto? lookups =null)
        {
            Id = user.Id;
            UserName = user.UserName;
            Activo = user.Activo;
            Rol = user.Rol;
            Lookups = lookups;
        }
        public static void Clear()
        {
            UserName = null;
            Id = null;
            Email = null;
            Lookups = null;
        }
    }
}
