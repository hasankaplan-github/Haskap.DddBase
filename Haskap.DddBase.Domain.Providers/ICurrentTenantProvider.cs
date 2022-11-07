using Haskap.DddBase.Domain.TenantAggregate;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Haskap.DddBase.Domain.Providers;

public interface ICurrentTenantProvider
{
    // https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs
    Guid? CurrentTenantId { get; }

    IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId);
}

// https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy
//public interface ITenantService
//{
//    string Tenant { get; }

//    void SetTenant(string tenant);

//    string[] GetTenants();

//    event TenantChangedEventHandler OnTenantChanged;
//}

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
