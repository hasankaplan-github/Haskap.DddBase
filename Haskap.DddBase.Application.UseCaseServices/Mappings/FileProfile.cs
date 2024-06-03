using AutoMapper;
using Haskap.DddBase.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.Mappings;

internal class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<Domain.Common.File, FileOutputDto>();
    }
}
