using AutoMapper;
using Haskap.DddBase.Application.Contracts.Tenants;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application.Dtos.Tenants;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.TenantAggregate;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.Tenants;
public class TenantService : UseCaseService, ITenantService
{
    private readonly IBaseDbContext _baseDbContext;
    private readonly IMapper _mapper;

    public TenantService(
        IBaseDbContext baseDbContext,
        IMapper mapper)
    {
        _baseDbContext = baseDbContext;
        _mapper = mapper;
    }

    public async Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken)
    {
        var toBeDeleted = await _baseDbContext.Tenant
            .Where(x=>x.Id == inputDto.TenantId)
            .FirstAsync(cancellationToken);

        toBeDeleted.MarkAsDeleted();

        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<TenantOutputDto>> GetAllForLoginViewAsync(CancellationToken cancellationToken)
    {
        var tenants = await _baseDbContext.Tenant
            .ToListAsync(cancellationToken);

        var output = _mapper.Map<List<TenantOutputDto>>(tenants);
        output = output.Prepend(new TenantOutputDto { Id = null, Name = "Host" }).ToList();

        return output;
    }

    public async Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken)
    {
        var newTenant = new Tenant(GuidGenerator.CreateSimpleGuid(), inputDto.Name, _baseDbContext.Tenant);

        await _baseDbContext.Tenant.AddAsync(newTenant, cancellationToken);
        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = _baseDbContext.Tenant.AsQueryable();

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

        var tenant = await _baseDbContext.Tenant
            .Where(x => x.Id == tenantId)
            .FirstAsync(cancellationToken);

        var output = _mapper.Map<TenantOutputDto>(tenant);

        return output;
    }

    public async Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var tenant = await _baseDbContext.Tenant
            .Where(x => x.Id == inputDto.TenantId)
            .FirstAsync(cancellationToken);

        tenant.SetName(inputDto.NewName, _baseDbContext.Tenant);

        await _baseDbContext.SaveChangesAsync(cancellationToken);
    }
}
