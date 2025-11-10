using Microsoft.Extensions.Localization;

namespace Haskap.DddBase.Application.Contracts.Localization;

public interface IDbStringLocalizer : IUseCaseService, IStringLocalizer
{
}
