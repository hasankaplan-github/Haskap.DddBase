using AutoMapper;
using Haskap.DddBase.Modules.Tenants.Application.Dtos.Tenants;
using Haskap.DddBase.Modules.Tenants.Domain.TenantAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.Tenants.Application.UseCaseServices.Mappings;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantOutputDto>();
    }
}
