using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.UserAggregate;
public class UserRole : Entity<Guid>
{
    private UserRole() { }

    public UserRole(Guid id)
        : base(id) { }

    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
