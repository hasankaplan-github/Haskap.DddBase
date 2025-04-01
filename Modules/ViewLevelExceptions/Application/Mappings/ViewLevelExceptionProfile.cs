using AutoMapper;
using Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ViewLevelExceptions.Application.UseCaseServices.Mappings;

public class ViewLevelExceptionProfile : Profile
{
    public ViewLevelExceptionProfile()
    {
        CreateMap<ViewLevelException, ViewLevelExceptionOutputDto>();
    }
}
