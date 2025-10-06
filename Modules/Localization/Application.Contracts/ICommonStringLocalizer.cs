using Haskap.DddBase.Application.Contracts;
using Microsoft.Extensions.Localization;

namespace Modules.Localization.Application.Contracts;
public interface ICommonStringLocalizer : IUseCaseService, IStringLocalizer
{
    IList<IStringLocalizer>? Localizers { get; }

    void SetStringLocalizers(params IList<IStringLocalizer> localizers);
}
