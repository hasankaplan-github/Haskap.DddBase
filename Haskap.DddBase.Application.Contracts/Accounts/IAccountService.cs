using Haskap.DddBase.Application.Dtos.Accounts;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Application.Dtos.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.Contracts.Accounts;
public interface IAccountService
{
    Task ChangePasswordAsync(ChangePasswordInputDto inputDto, CancellationToken cancellationToken = default);
    Task<LoginOutputDto> LoginAsync(LoginInputDto inputDto, CancellationToken cancellationToken = default);
    Task<HashSet<string>> GetAllPermissionsAsync(GetAllPermissionsInputDto inputDto, CancellationToken cancellationToken = default);
    Task<UpdateAccountOutputDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateAsync(Dtos.Accounts.UpdateInputDto inputDto, CancellationToken cancellationToken);
    Task<JqueryDataTableResult> SearchAsync(Dtos.Accounts.SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken);
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken);
    Task<HashSet<string>> GetUserPermissionsAsync(GetUserPermissionsInputDto inputDto, CancellationToken cancellationToken = default);
    Task UpdatePermissionsAsync(Dtos.Accounts.UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken);
    Task<List<RoleOutputDto>> GetRolesAsync(GetRolesInputDto inputDto, CancellationToken cancellationToken);
    Task UpdateRolesAsync(UpdateRolesInputDto inputDto, CancellationToken cancellationToken);
    Task ToggleActiveStatusAsync(Guid userId, CancellationToken cancellationToken);
    Task<LoginOutputDto> LoginAsAsync(LoginAsInputDto inputDto, CancellationToken cancellationToken);
}
