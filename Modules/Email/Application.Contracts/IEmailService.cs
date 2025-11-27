using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Application.Dtos.Common;
using MimeKit;

namespace Modules.Email.Application.Contracts;
public interface IEmailService : IUseCaseService
{
    Task SendInBulkAsync(SmtpClientSettingsDto smtpClientSettings, IList<MimeMessage> emailMessages, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken);
    Task SendAsync(SmtpClientSettingsDto smtpClientSettings, MimeMessage emailMessage, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken);
    MimeMessage Resolve<TEmailResolver>(IEmailParameters emailParameters)
        where TEmailResolver : IEmailResolver;
}
