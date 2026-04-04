using LD.Contracts.DTOs;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.Responses;
using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD.Forms.Classes
{
    public static class UserData
    {
        public static string? Id { get; set; }
        public static string? UserName { get; set; }
        public static string? Name { get; set; }

        public static string? Email { get; set; }
        public static AuthorizationDto Authorization { get; set; }

        public static bool HasModule(int moduleId) =>
            Authorization?.Modules?.Any(m => m.ModuleId == moduleId
                || (m.SubModules?.Any(s => s.ModuleId == moduleId) ?? false)) ?? false;

        public static void SetUserData(GetMeReponse user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Name=user.Name;
            Authorization = user.Authorization?? new AuthorizationDto();
        }
        public static void Clear()
        {
            UserName = null;
            Id = null;
            Email = null;
        }
    }
}
