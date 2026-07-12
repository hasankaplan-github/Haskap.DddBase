using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Common.Exceptions;

namespace Haskap.DddBase.Domain.Common;
public class EmailAddress : ValueObject
{
    public Guid Id { get; init; }
    public string Value { get; private set; }

    private EmailAddress()
    {
    }

    public EmailAddress(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value), exceptionCreator: () => new EmailAddressIsEmptyException());

        value = value.Trim();
        Guard.Against.InvalidEmailAddress(value, nameof(value), exceptionCreator: () => new EmailAddressIsInvalidException());

        Value = value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
