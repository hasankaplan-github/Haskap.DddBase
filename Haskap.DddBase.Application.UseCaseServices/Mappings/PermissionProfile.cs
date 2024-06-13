using AutoMapper;
using Haskap.DddBase.Application.Dtos.Common;
using Haskap.DddBase.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.Mappings;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<Permission, PermissionOutputDto>();
    }
}
