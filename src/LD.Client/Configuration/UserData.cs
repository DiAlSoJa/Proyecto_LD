using LD.Contracts.DTOs.Auth;
using LD.Contracts.Responses;
namespace LD.Client.Configuration
{
    public static class UserData
    {
        public static string? Id { get; set; }
        public static string? UserName { get; set; }
        public static string? Name { get; set; }

        public static string? Email { get; set; }
        public static AuthorizationDto Authorization { get; set; }
        public static bool HasPermission(string key) =>
            Authorization?.Modules?.SelectMany(m => m.Permissions).Any(p => p.Key == key) ?? false;


        public static void SetUserData(GetMeReponse user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Name = user.Name;
            Authorization = user.Authorization ?? new AuthorizationDto();
        }
        public static void Clear()
        {
            UserName = null;
            Id = null;
            Email = null;
        }
    }
}
