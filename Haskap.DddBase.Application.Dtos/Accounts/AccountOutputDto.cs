using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.Dtos.Accounts;
public class AccountOutputDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string? SystemTimeZoneId { get; set; }
    public bool IsActive { get; set; }
    public string TenantName { get; set; }
    public Guid? TenantId { get; set; }
    public string? EmailAddress { get; set; }
}
