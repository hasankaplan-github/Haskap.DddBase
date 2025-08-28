using System.ComponentModel.DataAnnotations;

namespace Haskap.DddBase.Domain;
public abstract class AbstractValidator<T> : IValidatableObject
{
    public T Value { get; set; }

    public IEnumerable<ValidationResult> Validate(T value, IServiceProvider? serviceProvider = null, IDictionary<object, object?>? items = null)
    {
        Value = value;

        return Validate(new ValidationContext(this, serviceProvider, items));
    }

    public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
}