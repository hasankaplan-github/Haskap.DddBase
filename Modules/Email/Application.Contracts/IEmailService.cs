using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Application.Dtos.Common;
using Modules.Email.Application.Dtos;

namespace Modules.Email.Application.Contracts;
public interface IEmailService : IUseCaseService
{
    Task SendInBulkAsync(SmtpClientSettingsDto smtpClientSettings, IList<EmailMessageInputDto> emailMessages, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken);
    Task SendAsync(SmtpClientSettingsDto smtpClientSettings, EmailMessageInputDto emailMessage, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken);
    EmailMessageInputDto Resolve<TEmailResolver>(IEmailParameters emailParameters)
        where TEmailResolver : IEmailResolver;
}
