using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.Dtos.Roles;
public class UpdatePermissionsInputDto
{
    public Guid RoleId { get; set; }
    public List<string> CheckedPermissions { get; set; }
    public List<string> UncheckedPermissions { get; set; }
}