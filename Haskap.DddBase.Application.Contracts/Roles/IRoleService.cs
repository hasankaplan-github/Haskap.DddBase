using Haskap.DddBase.Application.Dtos.Common;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application.Dtos.Roles;

namespace Haskap.DddBase.Application.Contracts.Roles;
public interface IRoleService
{
    Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken);
    Task<List<RoleOutputDto>> GetAllAsync(CancellationToken cancellationToken);
    Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken);
    Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken);
    Task<RoleOutputDto> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken);
    Task UpdatePermissionsAsync(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken);
    Task<List<PermissionOutputDto>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken);
}
