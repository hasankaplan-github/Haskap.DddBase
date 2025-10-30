using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.Tenants.Application.Contracts.Tenants;
using Modules.Tenants.Application.Dtos.Tenants;
using Modules.Tenants.Application.Mappings;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantAggregate;
using Modules.Tenants.IntegrationEvents;
using Haskap.DddBase.Domain.Events;
using Modules.Tenants.Domain.TenantAggregate.Events;

namespace Modules.Tenants.Application.Tenants;
public class TenantService : UseCaseService, ITenantService
{
    private readonly ITenantsDbContext _tenantsDbContext;
    private readonly IEventPublisher _eventPublisher;

    public TenantService(
        ITenantsDbContext tenantsDbContext,
        IEventPublisher eventPublisher)
    {
        _tenantsDbContext = tenantsDbContext;
        _eventPublisher = eventPublisher;
    }

    public async Task<IList<TenantOutputDto>> GetByIdAsync(IList<Guid?> tenantIds, CancellationToken cancellationToken)
    {
        var tenants = await _tenantsDbContext.Tenant
            .Where(x => tenantIds.Contains(x.Id))
            .Select(x => x.ToTenantOutputDto())
            .ToListAsync(cancellationToken);

        if (tenantIds.Contains(null))
        {
            tenants.Add(new TenantOutputDto { Id = null, Name = "Host" });
        }
        
        return tenants;
    }

    public async Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken)
    {
        var toBeDeleted = await _tenantsDbContext.Tenant
            .Where(x=>x.Id == inputDto.TenantId)
            .FirstAsync(cancellationToken);

        toBeDeleted.MarkAsDeleted();

        await _tenantsDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new TenantSoftDeletedIntegrationEvent(toBeDeleted.Id), cancellationToken);
    }

    public async Task<List<TenantOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var tenants = (await _tenantsDbContext.Tenant
            .OrderBy(x => x.Name)
            .Select(x => x.ToTenantOutputDto())
            .ToListAsync(cancellationToken))
            .Prepend(new TenantOutputDto { Id = null, Name = "Host" })
            .ToList();

        return tenants;
    }

    public async Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken)
    {
        var newTenant = new Tenant(GuidGenerator.CreateSimpleGuid(), inputDto.Name, inputDto.ConnectionString, _tenantsDbContext.Tenant);

        await _tenantsDbContext.Tenant.AddAsync(newTenant, cancellationToken);
        await _tenantsDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new TenantCreatedDomainEvent(newTenant.ToTenantOutputDto()), cancellationToken);
        await _eventPublisher.PublishAsync(new TenantCreatedIntegrationEvent(newTenant.Id, newTenant.Name), cancellationToken);
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = _tenantsDbContext.Tenant.AsQueryable();

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

        var tenants = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var tenantOutputDtos = tenants.Select(x => x.ToTenantOutputDto()).ToList();

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = tenantOutputDtos
        };
    }

    public async Task<TenantOutputDto> GetByIdAsync(Guid? tenantId, CancellationToken cancellationToken)
    {
        if (tenantId is null)
        {
            return new TenantOutputDto { Id = null, Name = "Host" };
        }

        var tenant = await _tenantsDbContext.Tenant
            .Where(x => x.Id == tenantId)
            .FirstAsync(cancellationToken);

        var output = tenant.ToTenantOutputDto();

        return output;
    }

    public async Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var tenant = await _tenantsDbContext.Tenant
            .Where(x => x.Id == inputDto.TenantId)
            .FirstAsync(cancellationToken);

        tenant.SetName(inputDto.NewName, _tenantsDbContext.Tenant);
        tenant.ConnectionString = inputDto.NewConnectionString;

        await _tenantsDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new TenantUpdatedIntegrationEvent(tenant.Id, tenant.Name), cancellationToken);
    }
}
