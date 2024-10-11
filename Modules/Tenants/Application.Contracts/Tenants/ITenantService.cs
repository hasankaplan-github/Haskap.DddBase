using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Modules.Tenants.Application.Dtos.Tenants;

namespace Modules.Tenants.Application.Contracts.Tenants;
public interface ITenantService
{
    Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken);
    Task<List<TenantOutputDto>> GetAllForLoginViewAsync(CancellationToken cancellationToken);
    Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken);
    Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken);
    Task<TenantOutputDto> GetByIdAsync(Guid? tenantId, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken);
}
