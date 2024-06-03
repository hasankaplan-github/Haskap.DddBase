using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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