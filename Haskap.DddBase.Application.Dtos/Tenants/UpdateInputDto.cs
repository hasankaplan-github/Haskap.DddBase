﻿namespace Haskap.DddBase.Application.Dtos.Tenants;

public class UpdateInputDto
{
    public Guid TenantId { get; set; }
    public string NewName { get; set; }
}