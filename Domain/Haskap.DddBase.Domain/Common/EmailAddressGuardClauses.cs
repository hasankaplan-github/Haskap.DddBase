using Ardalis.GuardClauses;

namespace Haskap.DddBase.Domain.Common;
internal static class EmailAddressGuardClauses
{
    public static string InvalidEmailAddress(this IGuardClause guardClause, string input, string? parameterName = null, string? message = null, Func<Exception>? exceptionCreator = null)
    {
        var emailAddressValidator = new EmailAddressValidator();
        var valiationResults = emailAddressValidator.Validate(input);

        if (valiationResults?.Any() == true)
            throw exceptionCreator?.Invoke() ?? new ArgumentException(message, parameterName);

        return input;
    }
}