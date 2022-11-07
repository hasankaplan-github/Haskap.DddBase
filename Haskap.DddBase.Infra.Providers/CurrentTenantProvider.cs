using Haskap.DddBase.Domain.TenantAggregate;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;

// bu provider scoped olarak tanımlanmalı ve middleware içinde ilk kez set edilmeli.
public class CurrentTenantProvider : ICurrentTenantProvider
{
    public Guid? CurrentTenantId { get; private set; } = Tenant.EmptyTenantId;
    //private static readonly AsyncLocal<Tenant?> _currentTenant = new AsyncLocal<Tenant?>();

    public IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId)
    {
        var oldTenantId = CurrentTenantId;
        CurrentTenantId = newCurrentTenantId;
        return new DisposeAction(() =>
        {
            CurrentTenantId = oldTenantId;
        });
    }
}

///////////////////////////////////////
/// 
//public class MultiTenancyMiddleware
//{
//    private readonly RequestDelegate _next;

//    public MultiTenancyMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task Invoke(HttpContext httpContext)
//    {
//        using (TenantInfo.Change(FindTenant(httpContext)))
//        {
//            await _next(httpContext);
//        }
//    }

//    private TenantInfo FindTenant(HttpContext httpContext)
//    {
//        var tenantId = FindFromClaims(httpContext) ??
//                       FindFromDomain(httpContext) ??
//                       FindFromHeader(httpContext) ??
//                       FindFromCookie(httpContext);

//        if (tenantId == null)
//        {
//            return null;
//        }

//        return new TenantInfo(Guid.Parse(tenantId));
//    }

//    private static string FindFromClaims(HttpContext httpContext)
//    {
//        return httpContext.User.FindFirstValue("_tenantId");
//    }

//    private string FindFromDomain(HttpContext httpContext)
//    {
//        return null;
//    }

//    private string FindFromHeader(HttpContext httpContext)
//    {
//        return httpContext.Request.Headers["_tenantId"].FirstOrDefault();
//    }

//    private string FindFromCookie(HttpContext httpContext)
//    {
//        return httpContext.Request.Cookies["_tenantId"];
//    }
//}
