using Haskap.DddBase.Domain.Core.TenantAggregate;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infrastructure.Providers;

public class CurrentTenantProvider
{
    public static bool MultiTenancyIsEnabled 
    {
        get
        {
            return multiTenancyIsEnabled.Value;
        }
        set
        {
            multiTenancyIsEnabled.Value = value;
        }
    }
    private static readonly AsyncLocal<bool> multiTenancyIsEnabled = new AsyncLocal<bool>();
    public bool CurrentTenantExists 
    { 
        get 
        { 
            return CurrentTenantInstance != null;
        } 
    }

    public Tenant? CurrentTenantInstance { get; set; }

    // CurrentTenant middleware içinde set edilmesi gerekiyor. Set edilecek sırada öncelikli olarak üstteki CurrentTenantInstance olmalı.
    // Bu provider singleton olarak tanımlanmalı.
    //
    // Middlerware içinde CurrentTenant set edilirken önce CurrentTenantInstance' a bakılacak.
    // Eğer CurrentTenantInstance null (CurrentTenantExists==false) ise login methoduna gidiyor demektir.
    // Login mehodunda CurrentTenantInstance set edilecek, eğer CurrentTenant ile başka işlemler yapılacaksa Change methodu ile CurrentTenant' da set edilecek.
    // Sonraki requestlerde middleware içinde CurrentTenant set edilmiş olacak ve her yerde bu kullanılacak.
    // https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs
    public static Tenant? CurrentTenant 
    {
        get 
        {
            return currentTenant.Value;
        } 
        private set
        {
            currentTenant.Value = value;
        } 
    }
    private static readonly AsyncLocal<Tenant?> currentTenant = new AsyncLocal<Tenant?>();

    public static IDisposable ChangeCurrentTenant(Tenant? tenant)
    {
        var tempTenant = CurrentTenant;
        CurrentTenant = tenant;
        return new DisposeAction(() =>
        {
            CurrentTenant = tempTenant;
        });
    }
}
