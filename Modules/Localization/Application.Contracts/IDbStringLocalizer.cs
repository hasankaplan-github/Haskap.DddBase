using Haskap.DddBase.Application.Contracts;
using Microsoft.Extensions.Localization;

namespace Modules.Localization.Application.Contracts;
public interface IDbStringLocalizer : IUseCaseService, IStringLocalizer
{
}
