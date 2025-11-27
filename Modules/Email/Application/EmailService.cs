using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Modules.Email.Application.Contracts;

namespace Modules.Email.Application;
public class EmailService : UseCaseService, IEmailService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EmailService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SendInBulkAsync(SmtpClientSettingsDto smtpClientSettings, IList<MimeMessage> emailMessages, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(emailMessages, nameof(emailMessages));

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpClientSettings.Host, smtpClientSettings.Port, smtpClientSettings.UseSsl, cancellationToken);
        await client.AuthenticateAsync(emailAccountToAuthenticate.Username, emailAccountToAuthenticate.Password, cancellationToken);

        foreach (var emailMessage in emailMessages)
        {
            ValidateEmailMessage(emailMessage);

            try
            {
                await client.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception)
            {
            }
        }

        await client.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendAsync(SmtpClientSettingsDto smtpClientSettings, MimeMessage emailMessage, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken)
    {
        ValidateEmailMessage(emailMessage);

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpClientSettings.Host, smtpClientSettings.Port, smtpClientSettings.UseSsl, cancellationToken);
        await client.AuthenticateAsync(emailAccountToAuthenticate.Username, emailAccountToAuthenticate.Password, cancellationToken);
        await client.SendAsync(emailMessage, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private void ValidateEmailMessage(MimeMessage emailMessage)
    {
        if(emailMessage is null)
        {
            throw new ArgumentNullException(nameof(emailMessage), "Email message cannot be null!");
        }

        if (emailMessage.From is null || !emailMessage.From.Any())
        {
            throw new ArgumentException("From address cannot be null or empty!", nameof(emailMessage.From));
        }

        if (emailMessage.To is null || !emailMessage.To.Any())
        {
            throw new ArgumentException("To address cannot be null or empty!", nameof(emailMessage.To));
        }
    }

    public MimeMessage Resolve<TEmailResolver>(IEmailParameters emailParameters)
        where TEmailResolver : IEmailResolver
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var emailResolver = scope.ServiceProvider.GetRequiredService<TEmailResolver>() as IEmailResolver;
        if (emailResolver is null)
        {
            throw new InvalidOperationException($"The email resolver of type {typeof(TEmailResolver).FullName} could not be resolved.");
        }
        return emailResolver.Resolve(emailParameters);
    }
}
