using Microsoft.AspNetCore.Authorization;

namespace LD.Api.Authorization;

public class AnyPermissionRequirement : IAuthorizationRequirement
{
    public const string PolicyPrefix = "AnyPermission:";

    public IReadOnlyCollection<string> Permissions { get; }

    public AnyPermissionRequirement(IEnumerable<string> permissions)
    {
        Permissions = permissions
            .Where(permission => !string.IsNullOrWhiteSpace(permission))
            .Select(permission => permission.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static string BuildPolicyName(IEnumerable<string> permissions)
    {
        return PolicyPrefix + string.Join("|", permissions);
    }

    public static IReadOnlyCollection<string> ParsePolicyName(string policyName)
    {
        if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            return Array.Empty<string>();

        return policyName[PolicyPrefix.Length..]
            .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
