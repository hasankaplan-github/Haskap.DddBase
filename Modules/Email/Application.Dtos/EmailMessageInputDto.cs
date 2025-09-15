namespace Modules.Email.Application.Dtos;
public class EmailMessageInputDto
{
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string BodyType { get; set; } = "text"; // or "html"
    public EmailMessageAddressInputDto From { get; set; }
    public List<EmailMessageAddressInputDto> To { get; set; } = new();
    public List<EmailMessageAddressInputDto> Cc { get; set; } = new();
    public List<EmailMessageAddressInputDto> Bcc { get; set; } = new();
}
