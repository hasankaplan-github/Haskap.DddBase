namespace Modules.Email.Domain.Shared.Consts;

public class SmtpClientSettings
{
    public const string SectionName = "SmtpClientSettings";

    public string Host { get; init; }
    public int Port { get; init; }
    public bool UseSsl { get; init; }
}