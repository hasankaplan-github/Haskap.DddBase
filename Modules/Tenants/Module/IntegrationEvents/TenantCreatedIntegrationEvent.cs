using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.Tenants.Module.IntegrationEvents;
public record TenantCreatedIntegrationEvent(
    Guid TenantId,
    string TenantName) : IntegrationEvent;