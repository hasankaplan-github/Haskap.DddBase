using AutoMapper;
using Modules.Tenants.Application.Dtos.Tenants;
using Modules.Tenants.Domain.TenantAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Tenants.Application.UseCaseServices.Mappings;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantOutputDto>();
    }
}
