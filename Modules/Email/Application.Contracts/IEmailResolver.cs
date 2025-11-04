using Modules.Email.Application.Dtos;

namespace Modules.Email.Application.Contracts;
public interface IEmailResolver
{
    EmailMessageInputDto Resolve(IEmailParameters emailParameters);
}
