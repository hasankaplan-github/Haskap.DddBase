using AutoMapper;
using Haskap.DddBase.Application.Dtos.Common;
using Haskap.DddBase.Application.Dtos.Roles;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.RoleAggregate;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application.Contracts.Roles;
using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.Caching.Memory;
using Haskap.DddBase.Domain.UserAggregate.Events;

namespace Haskap.DddBase.Application.UseCaseServices.Roles;
public class RoleService : UseCaseService, IRoleService
{
    private readonly IBaseDbContext _baseDbContext;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public RoleService(
        IBaseDbContext baseDbContext,
        IMapper mapper,
        IMemoryCache memoryCache,
        IBaseCacheKeyProvider baseCacheKeyProvider,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _baseDbContext = baseDbContext;
        _mapper = mapper;
        _memoryCache = memoryCache;
        _baseCacheKeyProvider = baseCacheKeyProvider;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken)
    {
        var toBeDeleted = await _baseDbContext.Role
            .Where(x=>x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        _baseDbContext.Role.Remove(toBeDeleted);
        await _baseDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new RolePermissionsCacheContentUpdatedDomainEvent(toBeDeleted.Id), cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var roles = await _baseDbContext.Role
            .ToListAsync(cancellationToken);

        var output = _mapper.Map<List<RoleOutputDto>>(roles);

        return output;
    }

    public async Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken)
    {
        var newRole = new Role(
            GuidGenerator.CreateSimpleGuid(),
            inputDto.Name,
            _baseDbContext.Role);

        await _baseDbContext.Role.AddAsync(newRole, cancellationToken);
        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = _baseDbContext.Role.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (inputDto.Name is not null)
        {
            filtered = true;
            query = query.Where(x => x.Name.ToLower().Contains(inputDto.Name.ToLower()));
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
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Name);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.Name);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var roles = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var roleOutputDtos = _mapper.Map<List<RoleOutputDto>>(roles);

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = roleOutputDtos
        };
    }

    public async Task<RoleOutputDto> GetByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _baseDbContext.Role
            .Where(x => x.Id == roleId)
            .FirstAsync(cancellationToken);

        var output = _mapper.Map<RoleOutputDto>(role);

        return output;
    }

    public async Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var role = await _baseDbContext.Role
            .Where(x => x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        role.SetName(inputDto.NewName, _baseDbContext.Role);

        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePermissionsAsync(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        var role = await _baseDbContext.Role
            .Where(x => x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        role.UpdatePermissions(inputDto.UncheckedPermissions, inputDto.CheckedPermissions);

        await _baseDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new RolePermissionsCacheContentUpdatedDomainEvent(role.Id), cancellationToken);
    }

    public async Task<List<PermissionOutputDto>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _baseDbContext.Role
           .Where(x => x.Id == roleId)
           .FirstAsync(cancellationToken);

        var output = _mapper.Map<List<PermissionOutputDto>>(role.Permissions);

        return output;
    }
}

