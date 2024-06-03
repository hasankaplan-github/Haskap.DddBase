using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.RoleAggregate.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.RoleAggregate;
public class Role : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    private List<Permission> _permissions = new();
    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

    private Role()
    {
    }

    public Role(Guid id, string name, DbSet<Role> roleDbSet)
        : base(id)
    {
        SetName(name, roleDbSet);
    }

    public void SetName(string name, DbSet<Role> roleDbSet)
    {
        Guard.Against.NullOrWhiteSpace(name);

        var exist = roleDbSet.Where(x => x.Name.ToLower().Equals(name.ToLower()))
            .Any();

        if (exist)
        {
            throw new DuplicateRoleNameException();
        }

        Name = name;
    }

    public void AddPermission(string permissionName)
    {
        _permissions.Add(new Permission(permissionName));
    }

    public void RemovePermission(string permissionName)
    {
        var toBeRemoved = _permissions.FirstOrDefault(x => x.Name.Equals(permissionName));
        _permissions.Remove(toBeRemoved);
    }

    public void RemovePermission(Permission permission)
    {
        _permissions.Remove(permission);
    }

    public void AddPermissions(IEnumerable<string> checkedPermissions)
    {
        if (checkedPermissions is null || !checkedPermissions.Any())
        {
            return;
        }

        var toBeAdded = checkedPermissions
           .Except(_permissions.Select(x => x.Name))
           .ToList();

        foreach (var permissionName in toBeAdded)
        {
            AddPermission(permissionName);
        }
    }

    public void RemovePermissions(IEnumerable<string> uncheckedPermissions)
    {
        if (uncheckedPermissions is null || !uncheckedPermissions.Any())
        {
            return;
        }

        if (!_permissions.Any())
        {
            return;
        }

        var toBeDeleted = _permissions
            .IntersectBy(uncheckedPermissions, x => x.Name)
            .ToList();

        foreach (var permission in toBeDeleted)
        {
            RemovePermission(permission);
        }
    }

    public void UpdatePermissions(IEnumerable<string> uncheckedPermissions, IEnumerable<string> checkedPermissions)
    {
        RemovePermissions(uncheckedPermissions);

        AddPermissions(checkedPermissions);
    }
}
