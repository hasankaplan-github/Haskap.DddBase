using System.ComponentModel.DataAnnotations;

namespace Haskap.DddBase.Domain;

public interface IValidator<T>
{
    IServiceProvider? ServiceProvider { get; set; }
    IDictionary<object, object?>? Items { get; set; }

    IEnumerable<ValidationResult> Validate(T value);
    void ValidateAndThrow(T value);
}

public abstract class AbstractValidator<T> : IValidator<T>
{
    public IServiceProvider? ServiceProvider { get; set; }
    public IDictionary<object, object?>? Items { get; set; }

    protected AbstractValidator(
        IServiceProvider? serviceProvider = null,
        IDictionary<object, object?>? items = null)
    {
        ServiceProvider = serviceProvider;
        Items = items;
    }

    public abstract IEnumerable<ValidationResult> Validate(T value);

    public void ValidateAndThrow(T value)
    {
        var results = Validate(value).ToList();
        if (results.Any())
        {
            throw new ValidationException(
                $"Validation failed for {typeof(T).Name}: {string.Join(", ", results.Select(r => r.ErrorMessage))}");
        }
    }
}