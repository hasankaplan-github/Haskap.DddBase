using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Shared.Consts;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Haskap.DddBase.Domain.UserAggregate.Exceptions;
using System.Threading;
using Haskap.DddBase.Utilities.Guids;

namespace Haskap.DddBase.Domain.UserAggregate;

[AddAuditHistoryLog]
public class User : AggregateRoot<Guid>, ISoftDeletable, IIsActive, IHasMultiTenant
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? EmailAddress { get; set; }
    public Credentials Credentials { get; private set; }
    public string? SystemTimeZoneId { get; private set; }

    private List<Permission> _permissions = new();
    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

    private List<UserRole> _roles = new();
    public IReadOnlyList<UserRole> Roles => _roles.AsReadOnly();

    public bool IsDeleted { get; set; }
    public Guid? TenantId { get; set; }
    public bool IsActive { get; set; }

    private User()
    { }

    public User(Guid id, string firstName, string lastName, string? emailAddress, Credentials credentials, string? systemTimeZoneId, Guid? tenantId, DbSet<User> userDbSet)
        : base(id)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        EmailAddress = emailAddress;
        SetCredentials(credentials, userDbSet);
        SetSystemTimeZoneId(systemTimeZoneId);
        TenantId = tenantId;
    }

    public void AddPermission(string permissionName)
    {
        _permissions.Add(new Permission(permissionName));
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

    public void RemovePermission(string permissionName)
    {
        var toBeRemoved = _permissions.FirstOrDefault(x => x.Name.Equals(permissionName));
        _permissions.Remove(toBeRemoved);
    }

    public void RemovePermission(Permission permission)
    {
        _permissions.Remove(permission);
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

    public void SetFirstName(string firstName)
    {
        Guard.Against.NullOrWhiteSpace(firstName, message: "İsim boş olamaz!");
        Guard.Against.InvalidInput(firstName, nameof(firstName), x => x.Length<=UserConsts.MaxFirstNameLength, message: "İsim 100 karakterden fazla olamaz!");
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        Guard.Against.NullOrWhiteSpace(lastName, message: "Soyisim boş olamaz!");
        Guard.Against.InvalidInput(lastName, nameof(lastName), x => x.Length<= UserConsts.MaxFirstNameLength, message: "Soyisim 100 karakterden fazla olamaz!");
        LastName = lastName;
    }

    public void SetCredentials(Credentials credentials, DbSet<User> userDbSet)
    {
        Guard.Against.Null(credentials);

        var duplicateUserName = userDbSet
            .Where(x => x.Id != Id && x.Credentials.UserName.ToLower().Equals(credentials.UserName.ToLower()))
            .Any();

        if (duplicateUserName)
        {
            throw new DuplicateUserNameException();
        }

        Credentials = credentials;
    }

    public void SetSystemTimeZoneId(string? systemTimeZoneId)
    {
        if (systemTimeZoneId is not null)
        {
            Guard.Against.InvalidInput(systemTimeZoneId, nameof(systemTimeZoneId), x => x.Length <= UserConsts.MaxSystemTimeZoneIdLength, "SystemTimeZoneId 150 karakterden fazla olamaz!");
        }

        SystemTimeZoneId = systemTimeZoneId;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void AddRole(Guid roleId)
    {
        _roles.Add(new UserRole(GuidGenerator.CreateSimpleGuid()) { RoleId = roleId, UserId = Id });
    }

    public void AddRoles(IEnumerable<Guid> checkedRoleIds)
    {
        if (checkedRoleIds is null || !checkedRoleIds.Any())
        {
            return;
        }

        var toBeAdded = checkedRoleIds
            .Except(_roles.Select(x => x.RoleId))
            .ToList();

        foreach (var roleId in toBeAdded)
        {
            AddRole(roleId);
        }
    }

    public void RemoveRole(Guid roleId)
    {
        var toBeRemoved = _roles.Where(x => x.RoleId == roleId).First();
        _roles.Remove(toBeRemoved);
    }

    public void RemoveRole(UserRole role)
    {
        _roles.Remove(role);
    }

    public void RemoveRoles(IEnumerable<Guid> uncheckedRoleIds)
    {
        if (uncheckedRoleIds is null || !uncheckedRoleIds.Any())
        {
            return;
        }

        if (!_roles.Any())
        {
            return;
        }

        var toBeDeleted = _roles
            .IntersectBy(uncheckedRoleIds, x => x.RoleId)
            .ToList();

        foreach (var userRole in toBeDeleted)
        {
            RemoveRole(userRole);
        }
    }

    public void UpdateRoles(IEnumerable<Guid> uncheckedRoleIds, IEnumerable<Guid> checkedRoleIds)
    {
        RemoveRoles(uncheckedRoleIds);

        AddRoles(checkedRoleIds);
    }

    public void UpdatePermissions(IEnumerable<string> uncheckedPermissions, IEnumerable<string> checkedPermissions)
    {
        RemovePermissions(uncheckedPermissions);

        AddPermissions(checkedPermissions);
    }

    public void ToggleActiveStatus()
    {
        IsActive = !IsActive;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
