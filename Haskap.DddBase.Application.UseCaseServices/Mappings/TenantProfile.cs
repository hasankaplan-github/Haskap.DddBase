using AutoMapper;
using Haskap.DddBase.Application.Dtos.Tenants;
using Haskap.DddBase.Domain.TenantAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.Mappings;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        CreateMap<Tenant, TenantOutputDto>();
    }
}
