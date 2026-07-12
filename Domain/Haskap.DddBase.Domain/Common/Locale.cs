using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Haskap.DddBase.Domain.Common;

public class Locale : ValueObject
{
    public string Value { get; private set; }

    public CultureInfo CultureInfo 
    {
        get
        {
            return new CultureInfo(Value);
        }
    
    }

    public string FlagIconCssClass
    {
        get
        {
            try
            {
                return "flag-icon flag-icon-" + new RegionInfo(Value).TwoLetterISORegionName.ToLower();
            }
            catch
            {
                return "flag-icon flag-icon-un";
            }
        }
    }

    private Locale()
    {
    }

    public Locale(string value)
    {
        Value = IsValidLocale(value) ? value : throw new CultureNotFoundException();
    }

    public static bool IsValidLocale(string value)
    {
        try
        {
            _ = new CultureInfo(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool RemoveDefinedLocale(Locale locale, [CallerArgumentExpression(nameof(locale))] string? definedLocalePropertyName = null)
    {
        return DefinedLocales.RemoveLocale(definedLocalePropertyName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    

    public static Locale CurrentLocale => new(CultureInfo.CurrentCulture.Name);
    public static Locale CurrentUiLocale => new(CultureInfo.CurrentUICulture.Name);
    //private static Locale? _default = null;
    public static Locale? Default = null;
    //{
    //    get => _default;
    //    set
    //    {
    //        _default = value ?? throw new ArgumentNullException(nameof(Default));
    //    }
    //}


    public static dynamic DefinedLocales = new InnerDefinedLocales();

    class InnerDefinedLocales : DynamicObject
    {
        private readonly Dictionary<string, Locale> _locales = new();

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            if (_locales.TryGetValue(binder.Name, out var locale))
            {
                result = locale;
                return true;
            }

            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            if (value is Locale locale)
            {
                _locales[binder.Name] = locale;
                return true;
            }

            return false;
        }

        public bool RemoveLocale(string propertyName)
        {
            propertyName = propertyName.Split('.')[^1]; // In case the caller uses CallerArgumentExpression, we need to extract the actual name.

            return _locales.Remove(propertyName);
        }
    }
}

/* Example of usage:
 * 
Locale.DefinedLocales.TrTr = new Locale("tr-TR");
Locale.DefinedLocales.EnUs = new Locale("en-US");
Locale.Default = Locale.DefinedLocales.TrTr;

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    List<CultureInfo> allCultures = [
        Locale.DefinedLocales.TrTr.CultureInfo,
        Locale.DefinedLocales.EnUs.CultureInfo
    ];

    options.DefaultRequestCulture = new RequestCulture(Locale.Default.CultureInfo);
    options.SupportedCultures = allCultures;
    options.SupportedUICultures = allCultures;
});

...

app.UseRequestLocalization();
 * 
 */