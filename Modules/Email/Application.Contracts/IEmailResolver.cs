using MimeKit;

namespace Modules.Email.Application.Contracts;
public interface IEmailResolver
{
    MimeMessage Resolve(IEmailParameters emailParameters);
}
