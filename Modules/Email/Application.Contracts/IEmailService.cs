using Haskap.DddBase.Application.Contracts;
using MimeKit;
using Modules.Email.Domain.Shared.Consts;

namespace Modules.Email.Application.Contracts;
public interface IEmailService : IUseCaseService
{
    Task SendInBulkAsync(SmtpClientSettings smtpClientSettings, IList<MimeMessage> emailMessages, EmailAccount emailAccountToAuthenticate, CancellationToken cancellationToken);
    Task SendAsync(SmtpClientSettings smtpClientSettings, MimeMessage emailMessage, EmailAccount emailAccountToAuthenticate, CancellationToken cancellationToken);
    MimeMessage Resolve<TEmailResolver>(IEmailParameters emailParameters)
        where TEmailResolver : IEmailResolver;
}
