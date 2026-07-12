using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Haskap.DddBase.Presentation.CustomAuthorization;
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; }

    public string? DisplayText { get; }

    public PermissionRequirement(string name, string? displayText = null)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        DisplayText = displayText;
    }
}

public class PermissionRequirementEqualityComparer : IEqualityComparer<PermissionRequirement>
{
    public bool Equals(PermissionRequirement? x, PermissionRequirement? y)
    {
        if (x is null || y is null) return false;

        return x.Name == y.Name;
    }

    public int GetHashCode([DisallowNull] PermissionRequirement obj)
    {
        return obj.Name.GetHashCode();
    }
}