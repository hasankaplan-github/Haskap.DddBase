using AutoMapper;
using Haskap.DddBase.Application.Dtos.Accounts;
using Haskap.DddBase.Application.Dtos.Roles;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.UserAggregate;
using Haskap.DddBase.Domain.UserAggregate.Exceptions;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application.Contracts.Accounts;
using Microsoft.Extensions.Caching.Memory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Haskap.DddBase.Domain.UserAggregate.Events;

namespace Haskap.DddBase.Application.UseCaseServices.Accounts;
public class AccountService : UseCaseService, IAccountService
{
    private readonly IBaseDbContext _baseDbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly UserDomainService _userDomainService;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IIsActiveGlobalQueryFilterProvider _isActive;
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;


    public AccountService(
        IBaseDbContext baseDbContext,
        IMapper mapper,
        ICurrentUserIdProvider currentUserIdProvider,
        UserDomainService userDomainService,
        ICurrentTenantProvider currentTenantProvider,
        IIsActiveGlobalQueryFilterProvider isActive,
        IMemoryCache memoryCache,
        IBaseCacheKeyProvider baseCacheKeyProvider)
    {
        _baseDbContext = baseDbContext;
        _mapper = mapper;
        _currentUserIdProvider = currentUserIdProvider;
        _userDomainService = userDomainService;
        _currentTenantProvider = currentTenantProvider;
        _isActive = isActive;
        _memoryCache = memoryCache;
        _baseCacheKeyProvider = baseCacheKeyProvider;
    }

    public async Task ChangePasswordAsync(ChangePasswordInputDto inputDto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(inputDto.CurrentPassword))
        {
            throw new CurrentPasswordEmptyException();
        }

        if (string.IsNullOrWhiteSpace(inputDto.NewPassword))
        {
            throw new NewPasswordEmptyException();
        }

        if (inputDto.NewPassword == inputDto.CurrentPassword)
        {
            throw new SamePasswordException();
        }

        if (inputDto.NewPassword != inputDto.NewPasswordConfirmation)
        {
            throw new PasswordConfirmationMismatchException();
        }

        var user = await _baseDbContext.User
            .FindAsync(new object[] { _currentUserIdProvider.CurrentUserId!.Value }, cancellationToken);

        var hashedCurrentPassword = Password.ComputeHash(inputDto.CurrentPassword, user.Credentials.Password.Salt);
        if (user.Credentials.Password.HashedValue != hashedCurrentPassword)
        {
            throw new CurrentPasswordMismatchException();
        }

        var newPassword = new Password(inputDto.NewPassword, Salt.Generate());
        var newCredentials = new Credentials(user.Credentials.UserName, newPassword);

        user.SetCredentials(newCredentials, _baseDbContext.User);
        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoginOutputDto> LoginAsync(LoginInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var user = await _baseDbContext.User
            .Where(x => x.Credentials.UserName == inputDto.UserName)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new WrongUserNameOrPasswordException();
        }

        var hashedPassword = Password.ComputeHash(inputDto.Password, user.Credentials.Password.Salt);
        if (hashedPassword != user.Credentials.Password.HashedValue)
        {
            throw new WrongUserNameOrPasswordException();
        }

        var output = new LoginOutputDto
        {
            UserId = user.Id,
            UserFirstName = user.FirstName,
            UserLastName = user.LastName,
            UserSystemTimeZoneId = user.SystemTimeZoneId
        };

        return output;
    }

    public async Task<HashSet<string>> GetAllPermissionsForCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = _currentUserIdProvider.CurrentUserId.Value }, cancellationToken);
    }

    public async Task<HashSet<string>> GetAllPermissionsAsync(GetAllPermissionsInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _memoryCache.GetOrCreateAsync(_baseCacheKeyProvider.GetAllPermissionsCacheKey(inputDto.UserId), async cacheEntry =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);

            var allPermissions = await _userDomainService.GetAllPermissionsAsync(inputDto.UserId, cancellationToken);

            return allPermissions;
        });

        return cachedValue;
    }

    public async Task<HashSet<string>> GetUserPermissionsForCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        return await GetUserPermissionsAsync(new GetUserPermissionsInputDto { UserId = _currentUserIdProvider.CurrentUserId.Value }, cancellationToken);
    }

    public async Task<HashSet<string>> GetUserPermissionsAsync(GetUserPermissionsInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _memoryCache.GetOrCreateAsync(_baseCacheKeyProvider.GetUserPermissionsCacheKey(inputDto.UserId), async cacheEntry =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);

            var userPermissions = await _userDomainService.GetUserPermissionsAsync(inputDto.UserId, cancellationToken);

            return userPermissions;
        });

        return cachedValue;
    }

    public async Task<UpdateAccountOutputDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _baseDbContext.User
            .Where(x => x.Id == userId)
            .FirstAsync(cancellationToken);

        var output = _mapper.Map<UpdateAccountOutputDto>(user);

        return output;
    }

    public async Task UpdateAsync(Dtos.Accounts.UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _baseDbContext.User
            .Where(x => x.Id == _currentUserIdProvider.CurrentUserId.Value)
            .FirstAsync(cancellationToken);

        var hashedCurrentPassword = Password.ComputeHash(inputDto.CurrentPassword, user.Credentials.Password.Salt);

        if (user.Credentials.Password.HashedValue != hashedCurrentPassword)
        {
            throw new CurrentPasswordMismatchException();
        }

        var newCredentials = new Credentials(inputDto.UserName, new Password(inputDto.CurrentPassword, Salt.Generate()));

        user.SetFirstName(inputDto.FirstName);
        user.SetLastName(inputDto.LastName);
        user.SetCredentials(newCredentials, _baseDbContext.User);
        user.SetSystemTimeZoneId(inputDto.SystemTimeZoneId);

        await _baseDbContext.SaveChangesAsync();
    }


    public async Task<JqueryDataTableResult> SearchAsync(Dtos.Accounts.SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        using var _ = _isActive.Disable();

        var query = (from user in _baseDbContext.User
                     from tenant in _baseDbContext.Tenant.Where(x => x.Id == user.TenantId).DefaultIfEmpty()
                     select new AccountOutputDto
                     {
                         Id = user.Id,
                         FirstName = user.FirstName, 
                         LastName = user.LastName,
                         IsActive = user.IsActive,
                         SystemTimeZoneId = user.SystemTimeZoneId,
                         UserName = user.Credentials.UserName,
                         TenantId = user.TenantId,
                         TenantName = tenant.Name ?? "Host"
                     });

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (inputDto.FirstName is not null)
        {
            filtered = true;
            query = query.Where(x => x.FirstName.Contains(inputDto.FirstName));
        }

        if (inputDto.LastName is not null)
        {
            filtered = true;
            query = query.Where(x => x.LastName.Contains(inputDto.LastName));
        }

        if (inputDto.UserName is not null)
        {
            filtered = true;
            query = query.Where(x => x.UserName.Contains(inputDto.UserName));
        }

        if (inputDto.IsActive is not null)
        {
            filtered = true;
            query = query.Where(x => x.IsActive == inputDto.IsActive);
        }


        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        if (jqueryDataTableParam.Order.Any())
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 0)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.FirstName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.FirstName);
                }
            }
            else if (columnIndex == 1)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.LastName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.LastName);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.UserName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.UserName);
                }
            }
            else if (columnIndex == 3)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.SystemTimeZoneId);
                }
                else
                {
                    query = query.OrderByDescending(x => x.SystemTimeZoneId);
                }
            }
            else if (columnIndex == 4)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.IsActive);
                }
                else
                {
                    query = query.OrderByDescending(x => x.IsActive);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.FirstName);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var accountOutputDtos = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = accountOutputDtos
        };
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _baseDbContext.User
            .Where(x => x.Id == userId)
            .FirstAsync(cancellationToken);

        user.MarkAsDeleted();

        await _baseDbContext.SaveChangesAsync();
    }

    public async Task UpdatePermissionsAsync(Dtos.Accounts.UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _baseDbContext.User
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        user.UpdatePermissions(inputDto.UncheckedPermissions, inputDto.CheckedPermissions);

        await _baseDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new UserPermissionsCacheContentUpdatedDomainEvent(user.Id), cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetRolesForCurrentUserAsync(CancellationToken cancellationToken)
    {
        return await GetRolesAsync(new GetRolesInputDto { UserId = _currentUserIdProvider.CurrentUserId.Value }, cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetRolesAsync(GetRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        var roles = (from user in _baseDbContext.User
                     join userRole in _baseDbContext.UserRole on user.Id equals userRole.UserId
                     join role in _baseDbContext.Role on userRole.RoleId equals role.Id
                     where user.Id == inputDto.UserId
                     select new RoleOutputDto
                     {
                         Id = role.Id,
                         Name = role.Name
                     })
                    .ToList();

        return roles;
    }

    public async Task UpdateRolesAsync(Dtos.Accounts.UpdateRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _baseDbContext.User
            .Include(x => x.Roles)
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        user.UpdateRoles(inputDto.UncheckedRoles, inputDto.CheckedRoles);

        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ToggleActiveStatusAsync(Guid userId, CancellationToken cancellationToken)
    {
        using var _ = _isActive.Disable();

        var user = await _baseDbContext.User
            .Where(x => x.Id == userId)
            .FirstAsync(cancellationToken);

        user.ToggleActiveStatus();

        await _baseDbContext.SaveChangesAsync();
    }
}