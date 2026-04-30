using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Shared.Enums;
using Haskap.DddBase.Domain.Shared.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.Providers;
using Modules.SpecialDayBase.Domain.Shared.Consts;
using Modules.SpecialDayBase.Domain.Shared.Enums;
using Modules.SpecialDayBase.Domain.SpecialDayAggregate;
using Modules.SpecialDayBase.Infra.Providers;
using Modules.TurkiyeSpecialDay.Application.Contracts;
using Modules.TurkiyeSpecialDay.Domain.Providers;

namespace Modules.TurkiyeSpecialDay.Infra.Providers;
public class TurkiyeSpecialDayCalendarProvider : SpecialDayCalendarProvider, ITurkiyeSpecialDayCalendarProvider
{
    public override Country ForCountry => Country.Turkiye;
    public override IWeekendProvider? WeekendProvider => SpecialDayBase.Infra.Providers.WeekendProvider.Universal;

    private readonly ITurkiyeSpecialDayModule _turkiyeSpecialDayModule;

    public TurkiyeSpecialDayCalendarProvider(
        [FromKeyedServices("FixedAndOccurrenceBased")] ISpecialDaySpecificationEvaluator fixedAndOccurrenceBasedSpecialDaySpecificationEvaluator,
        [FromKeyedServices("FixedWithExactDate")] ISpecialDaySpecificationEvaluator fixedWithExactDateSpecialDaySpecificationEvaluator,
        [FromKeyedServices("OneTimeAndOccurrenceBased")] ISpecialDaySpecificationEvaluator oneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator,
        [FromKeyedServices("OneTimeWithExactDate")] ISpecialDaySpecificationEvaluator oneTimeWithExactDateSpecialDaySpecificationEvaluator,
        ITurkiyeSpecialDayModule turkiyeSpecialDayModule,
        IStringLocalizer<CommonTexts> localizer)
        : base(
            fixedAndOccurrenceBasedSpecialDaySpecificationEvaluator,
            fixedWithExactDateSpecialDaySpecificationEvaluator,
            oneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator,
            oneTimeWithExactDateSpecialDaySpecificationEvaluator,
            localizer)
    {
        _turkiyeSpecialDayModule = turkiyeSpecialDayModule;
    }

    public IEnumerable<SpecialDayOutputDto> NewYear(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 1,
            day: 1,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("New Year", Locale.DefinedLocales.EnUs), new Name("Yılbaşı Tatili", Locale.DefinedLocales.TrTr)],
            hasEveDay: true,
            eveDayType: EveDayType.HalfDay);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion);
    }

    public SpecialDayOutputDto NationalSovereigntyAndChildrensDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 4,
            day: 23,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("National Sovereignty And Childrens Day", Locale.DefinedLocales.EnUs), new Name("Ulusal Egemenlik ve Çocuk Bayramı", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto LabourDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 5,
            day: 1,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("Labour Day", Locale.DefinedLocales.EnUs), new Name("Emek Ve Dayanışma Günü", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public IEnumerable<SpecialDayOutputDto> Ramadan(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 10,
            day: 1,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: true,
            [new Name("Eid al-Fitr", Locale.DefinedLocales.EnUs), new Name("Ramazan Bayramı", Locale.DefinedLocales.TrTr)],
            hasEveDay: true,
            eveDayType: EveDayType.HalfDay,
            lengthInDays: 3);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion);
    }

    public SpecialDayOutputDto YouthAndSportsDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 5,
            day: 19,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("Youth And Sports Day", Locale.DefinedLocales.EnUs), new Name("Atatürk’ü Anma, Gençlik ve Spor Bayramı", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public IEnumerable<SpecialDayOutputDto> FeastOfSacrifices(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 12,
            day: 10,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: true,
            [new Name("Eid al-Adha", Locale.DefinedLocales.EnUs), new Name("Kurban Bayramı", Locale.DefinedLocales.TrTr)],
            hasEveDay: true,
            eveDayType: EveDayType.HalfDay,
            lengthInDays: 4);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion);
    }

    public SpecialDayOutputDto DemocracyAndNationalUnityDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 7,
            day: 15,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("Democracy And National Unity Day", Locale.DefinedLocales.EnUs), new Name("Demokrasi ve Milli Birlik Günü", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto VictoryDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 8,
            day: 30,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("Victory Day", Locale.DefinedLocales.EnUs), new Name("Zafer Bayramı", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto RepublicDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 10,
            day: 29,
            isHoliday: true,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("Republic Day", Locale.DefinedLocales.EnUs), new Name("Cumhuriyet Bayramı", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto ValentinesDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 2,
            day: 14,
            isHoliday: false,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("Valentine's Day", Locale.DefinedLocales.EnUs), new Name("Sevgililer Günü", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto MothersDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedAndOccurrenceBasedSpecialDay(
            month: 5,
            dayOfWeek: DayOfWeek.Sunday,
            occurrence: SpecialDayOccurrenceInAMonth.Second,
            isHoliday: false,
            group: SpecialDayConsts.Groups.None,
            [new Name("Mother's Day", Locale.DefinedLocales.EnUs), new Name("Anneler Günü", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto FathersDay(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedAndOccurrenceBasedSpecialDay(
            month: 6,
            dayOfWeek: DayOfWeek.Sunday,
            occurrence: SpecialDayOccurrenceInAMonth.Third,
            isHoliday: false,
            group: SpecialDayConsts.Groups.None,
            [new Name("Father's Day", Locale.DefinedLocales.EnUs), new Name("Babalar Günü", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }

    public SpecialDayOutputDto TheCommemorationDayOfAtatürk(int year)
    {
        var specialDaySpecificaion = SpecialDaySpecification.CreateFixedWithExactDateSpecialDay(
            month: 11,
            day: 10,
            isHoliday: false,
            group: SpecialDayConsts.Groups.None,
            useHijriCalendar: false,
            [new Name("The Commemoration Day of Atatürk", Locale.DefinedLocales.EnUs), new Name("Atatürk'ü Anma Günü", Locale.DefinedLocales.TrTr)]);

        return EvaluateSpecialDaySpecification(year, specialDaySpecificaion)[0];
    }



    public override IList<SpecialDayOutputDto> GetSpecialDays(int year)
    {
        if (!_turkiyeSpecialDayModule.IsEnabledAsync().GetAwaiter().GetResult())
        {
            return [];
        }

        List<SpecialDayOutputDto> specialDays = [
            ..NewYear(year),
            NationalSovereigntyAndChildrensDay(year),
            LabourDay(year),
            ..Ramadan(year),
            YouthAndSportsDay(year),
            ..FeastOfSacrifices(year),
            VictoryDay(year),
            RepublicDay(year),
            TheCommemorationDayOfAtatürk(year),
            ValentinesDay(year),
            MothersDay(year),
            FathersDay(year)
        ];

        if (year >= 2017)
        {
            specialDays.Add(DemocracyAndNationalUnityDay(year));
        }

        return specialDays;
    }
}
