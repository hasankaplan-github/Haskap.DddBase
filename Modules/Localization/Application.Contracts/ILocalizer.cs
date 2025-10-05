using Haskap.DddBase.Application.Contracts;

namespace Modules.Localization.Application.Contracts;
public interface ILocalizer : IUseCaseService
{
    string GetString(string key);
}
