using System.ComponentModel.DataAnnotations;

namespace Haskap.DddBase.Domain.Common;
internal class EmailAddressValidator : AbstractValidator<string>
{
    public EmailAddressValidator(
        IServiceProvider? serviceProvider = null,
        IDictionary<object, object?>? items = null)
        : base(serviceProvider, items)
    {
    }

    public override IEnumerable<ValidationResult> Validate(string value)
    {
        if (!System.Net.Mail.MailAddress.TryCreate(value, out _))
        {
            yield return new ValidationResult("Invalid Email Address!", ["EmailAddress"]);
        }
    }
}
