using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Application.Dtos.Common;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Modules.Localization.Application.Dtos;

namespace Modules.Localization.Application.Contracts;
public interface ILocalizationService : IUseCaseService
{
    Task AddLocalizationAsync(AddLocalizationInputDto input, CancellationToken cancellationToken = default);
    Task UpdateLocalizationAsync(UpdateLocalizationInputDto input, CancellationToken cancellationToken = default);
    Task DeleteLocalizationAsync(Guid localizationId, CancellationToken cancellationToken = default);
    Task<IList<SupportedLocaleOutputDto>> GetActiveSupportedLocalesAsync(CancellationToken cancellationToken = default);
    Task<IList<SupportedLocaleOutputDto>> GetAllSupportedLocalesAsync(CancellationToken cancellationToken = default);
    Task SetDefaultLocaleAsync(string localeValue, CancellationToken cancellationToken = default);
    Task AddSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default);
    Task RemoveSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default);
    Task ActivateSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default);
    Task DeactivateSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default);
    Task<LocaleOutputDto?> GetDefaultLocaleAsync(CancellationToken cancellationToken = default);
    Task<SelectedLocaleOutputDto?> GetSelectedLocaleForUser(Guid userId, CancellationToken cancellationToken = default);
    Task<JqueryDataTableResult> ListSupportedLocalesAsync(JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default);
    Task InvalidateLocaleCacheAsync(string localeValue, CancellationToken cancellationToken = default);
    Task InvalidateAllLocalesCachesAsync(CancellationToken cancellationToken = default);
}
