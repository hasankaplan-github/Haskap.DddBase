using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Modules.Email.Application.Contracts;
using Modules.Email.Application.Dtos;

namespace Modules.Email.Application;
public class EmailService : UseCaseService, IEmailService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EmailService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SendInBulkAsync(SmtpClientSettingsDto smtpClientSettings, IList<EmailMessageInputDto> emailMessages, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(emailMessages, nameof(emailMessages));

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpClientSettings.Host, smtpClientSettings.Port, smtpClientSettings.UseSsl, cancellationToken);
        await client.AuthenticateAsync(emailAccountToAuthenticate.Username, emailAccountToAuthenticate.Password, cancellationToken);

        foreach (var emailMessage in emailMessages)
        {
            ValidateEmailMessage(emailMessage);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailMessage.From.Name, emailMessage.From.Address));

            foreach (var to in emailMessage.To)
            {
                message.To.Add(new MailboxAddress(to.Name, to.Address));
            }

            if (emailMessage.Cc.Any())
            {
                foreach (var cc in emailMessage.Cc)
                {
                    message.Cc.Add(new MailboxAddress(cc.Name, cc.Address));
                }
            }

            if (emailMessage.Bcc.Any())
            {
                foreach (var bcc in emailMessage.Bcc)
                {
                    message.Bcc.Add(new MailboxAddress(bcc.Name, bcc.Address));
                }
            }

            message.Subject = emailMessage.Subject ?? string.Empty;

            var messageBody = emailMessage.Body ?? string.Empty;
            message.Body = emailMessage.BodyType.Equals("html", StringComparison.OrdinalIgnoreCase)
                ? new TextPart("html") { Text = messageBody }
                : new TextPart("plain") { Text = messageBody };

            try
            {
                await client.SendAsync(message, cancellationToken);
            }
            catch (Exception)
            {
            }
        }

        await client.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendAsync(SmtpClientSettingsDto smtpClientSettings, EmailMessageInputDto emailMessage, EmailAccountDto emailAccountToAuthenticate, CancellationToken cancellationToken)
    {
        ValidateEmailMessage(emailMessage);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailMessage.From.Name, emailMessage.From.Address));

        foreach (var to in emailMessage.To)
        {
            message.To.Add(new MailboxAddress(to.Name, to.Address));
        }

        if (emailMessage.Cc.Any())
        {
            foreach (var cc in emailMessage.Cc)
            {
                message.Cc.Add(new MailboxAddress(cc.Name, cc.Address));
            }
        }

        if (emailMessage.Bcc.Any())
        {
            foreach (var bcc in emailMessage.Bcc)
            {
                message.Bcc.Add(new MailboxAddress(bcc.Name, bcc.Address));
            }
        }

        message.Subject = emailMessage.Subject ?? string.Empty;

        var messageBody = emailMessage.Body ?? string.Empty;
        message.Body = emailMessage.BodyType.Equals("html", StringComparison.OrdinalIgnoreCase)
            ? new TextPart("html") { Text = messageBody }
            : new TextPart("plain") { Text = messageBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpClientSettings.Host, smtpClientSettings.Port, smtpClientSettings.UseSsl, cancellationToken);
        await client.AuthenticateAsync(emailAccountToAuthenticate.Username, emailAccountToAuthenticate.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private void ValidateEmailMessage(EmailMessageInputDto emailMessage)
    {
        if(emailMessage is null)
        {
            throw new ArgumentNullException(nameof(emailMessage), "Email message cannot be null!");
        }

        if (string.IsNullOrWhiteSpace(emailMessage.From?.Address))
        {
            throw new ArgumentException("From address cannot be null or empty!", nameof(emailMessage.From));
        }

        if (emailMessage.To is null || !emailMessage.To.Any())
        {
            throw new ArgumentException("To address cannot be null or empty!", nameof(emailMessage.To));
        }
    }

    public EmailMessageInputDto Resolve<TEmailResolver>(params IList<object> emailContentData)
        where TEmailResolver : IEmailResolver
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var emailResolver = scope.ServiceProvider.GetRequiredService<TEmailResolver>() as IEmailResolver;
        if (emailResolver is null)
        {
            throw new InvalidOperationException($"The email resolver of type {typeof(TEmailResolver).FullName} could not be resolved.");
        }
        return emailResolver.Resolve(emailContentData);
    }
}
