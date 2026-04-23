using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Utilities.Guids;
using Modules.SpecialDayBase.Domain.Shared.Consts;

namespace Modules.SpecialDayBase.Domain.SpecialDayAggregate;
public class Name : ValueObject
{
    public Guid Id { get; init; }
    public string Value { get; private set; }
    public Locale Locale { get; private set; }

    private Name()
    {
    }

    public Name(string value, Locale locale)
    {
        Guard.Against.NullOrEmpty(value, nameof(value));
        Guard.Against.InvalidInput(value, nameof(value), x => x.Length <= SpecialDayConsts.MaxNameLength, message: "İsim 500 karakterden fazla olamaz!");
        Guard.Against.Null(locale, nameof(locale));

        Id = GuidGenerator.CreateSimpleGuid();
        Value = value;
        Locale = locale;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Locale;
    }
}
