using AutoMapper;
using Haskap.DddBase.Application.Dtos.ViewLevelExceptions;
using Haskap.DddBase.Domain.ViewLevelExceptionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.Mappings;

public class ViewLevelExceptionProfile : Profile
{
    public ViewLevelExceptionProfile()
    {
        CreateMap<ViewLevelException, ViewLevelExceptionOutputDto>();
    }
}
