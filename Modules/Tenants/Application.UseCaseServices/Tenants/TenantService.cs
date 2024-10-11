﻿using AutoMapper;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application.UseCaseServices;
using Modules.Tenants.Application.Contracts.Tenants;
using Modules.Tenants.Application.Dtos.Tenants;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantAggregate;
using Modules.Tenants.IntegrationEvents;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;

namespace Modules.Tenants.Application.UseCaseServices.Tenants;
public class TenantService : UseCaseService, ITenantService
{
    private readonly ITenantsDbContext _tenantsDbContext;
    private readonly IMapper _mapper;

    public TenantService(
        ITenantsDbContext tenantsDbContext,
        IMapper mapper)
    {
        _tenantsDbContext = tenantsDbContext;
        _mapper = mapper;
    }

    public async Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken)
    {
        var toBeDeleted = await _tenantsDbContext.Tenant
            .Where(x=>x.Id == inputDto.TenantId)
            .FirstAsync(cancellationToken);

        toBeDeleted.MarkAsDeleted();

        await _tenantsDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new TenantSoftDeletedIntegrationEvent(toBeDeleted.Id), cancellationToken);
    }

    public async Task<List<TenantOutputDto>> GetAllForLoginViewAsync(CancellationToken cancellationToken)
    {
        var tenants = await _tenantsDbContext.Tenant
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var output = _mapper.Map<List<TenantOutputDto>>(tenants);
        output = output.Prepend(new TenantOutputDto { Id = null, Name = "Host" }).ToList();

        return output;
    }

    public async Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken)
    {
        var newTenant = new Tenant(GuidGenerator.CreateSimpleGuid(), inputDto.Name, _tenantsDbContext.Tenant);

        await _tenantsDbContext.Tenant.AddAsync(newTenant, cancellationToken);
        await _tenantsDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new TenantCreatedIntegrationEvent(newTenant.Id, newTenant.Name), cancellationToken);
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

        var tenantOutputDtos = _mapper.Map<List<TenantOutputDto>>(tenants);

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

        var output = _mapper.Map<TenantOutputDto>(tenant);

        return output;
    }

    public async Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var tenant = await _tenantsDbContext.Tenant
            .Where(x => x.Id == inputDto.TenantId)
            .FirstAsync(cancellationToken);

        tenant.SetName(inputDto.NewName, _tenantsDbContext.Tenant);

        await _tenantsDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new TenantUpdatedIntegrationEvent(tenant.Id, tenant.Name), cancellationToken);
    }
}
