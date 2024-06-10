using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.UserAggregate.Events;
public record RolePermissionsCacheContentUpdatedDomainEvent(Guid RoleId) : DomainEvent;