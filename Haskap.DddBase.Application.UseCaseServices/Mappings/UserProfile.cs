using AutoMapper;
using Haskap.DddBase.Application.Dtos.Accounts;
using Haskap.DddBase.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UpdateAccountOutputDto>()
            .ForMember(x => x.UserName, x => x.MapFrom(y => y.Credentials.UserName));
    }
}
