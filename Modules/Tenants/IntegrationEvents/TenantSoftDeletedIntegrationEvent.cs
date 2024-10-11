using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Tenants.IntegrationEvents;
public record TenantSoftDeletedIntegrationEvent(Guid TenantId) : IntegrationEvent;