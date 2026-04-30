using Haskap.DddBase.Domain.Shared.Enums;
using Microsoft.Extensions.Localization;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.Providers;
using Modules.SpecialDayBase.Domain.Shared.Consts;
using Modules.SpecialDayBase.Domain.SpecialDayAggregate;

namespace Modules.SpecialDayBase.Infra.Providers;
public abstract class SpecialDayCalendarProvider : ISpecialDayCalendarProvider
{
    protected readonly ISpecialDaySpecificationEvaluator FixedAndOccurrenceBasedSpecialDaySpecificationEvaluator;
    protected readonly ISpecialDaySpecificationEvaluator FixedWithExactDateSpecialDaySpecificationEvaluator;
    protected readonly ISpecialDaySpecificationEvaluator OneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator;
    protected readonly ISpecialDaySpecificationEvaluator OneTimeWithExactDateSpecialDaySpecificationEvaluator;
    protected readonly IStringLocalizer Localizer;

    public abstract Country ForCountry { get; }

    public virtual IWeekendProvider? WeekendProvider { get; } = null;

    protected SpecialDayCalendarProvider(
        ISpecialDaySpecificationEvaluator fixedAndOccurrenceBasedSpecialDaySpecificationEvaluator,
        ISpecialDaySpecificationEvaluator fixedWithExactDateSpecialDaySpecificationEvaluator,
        ISpecialDaySpecificationEvaluator oneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator,
        ISpecialDaySpecificationEvaluator oneTimeWithExactDateSpecialDaySpecificationEvaluator,
        IStringLocalizer localizer)
    {
        FixedAndOccurrenceBasedSpecialDaySpecificationEvaluator = fixedAndOccurrenceBasedSpecialDaySpecificationEvaluator;
        FixedWithExactDateSpecialDaySpecificationEvaluator = fixedWithExactDateSpecialDaySpecificationEvaluator;
        OneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator = oneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator;
        OneTimeWithExactDateSpecialDaySpecificationEvaluator = oneTimeWithExactDateSpecialDaySpecificationEvaluator;
        Localizer = localizer;
    }

    public abstract IList<SpecialDayOutputDto> GetSpecialDays(int year);

    public IList<SpecialDayOutputDto> GetSpecialDays(DateOnly startDate, DateOnly endDate)
    {
        var specialDays = new List<SpecialDayOutputDto>();
        for (var year = startDate.Year; year <= endDate.Year; year++)
        {
            specialDays.AddRange(GetSpecialDays(year)
                                  .Where(d => d.Date >= startDate && d.Date <= endDate));
        }
        return specialDays;
    }

    public bool IsSpecialDay(DateOnly date)
    {
        return GetSpecialDays(date.Year).Any(x => x.Date == date);
    }

    public bool TryGetSpecialDays(DateOnly day, out List<SpecialDayOutputDto> specialDays)
    {
        specialDays = GetSpecialDays(day.Year)
            .Where(x => x.Date == day)
            .ToList();

        return specialDays.Any();
    }

    public virtual string GetLocalizedGroupName(string group)
    {
        return Localizer["SpecialDayGroup:" + group];
    }

    protected virtual IList<SpecialDayOutputDto> EvaluateSpecialDaySpecification(int year, SpecialDaySpecification specialDaySpecification)
    {
        var specialDaysOutput = InnerEvaluate(year, specialDaySpecification);

        return specialDaysOutput.Select(x =>
        {
            x.DisplayName = x.Group == SpecialDayConsts.Groups.None 
                ? x.Name 
                : string.Format("({0}) {1}", GetLocalizedGroupName(x.Group), x.Name);

            return x;
        }).ToList();
    }

    private IList<SpecialDayOutputDto> InnerEvaluate(int year, SpecialDaySpecification specialDaySpecification)
    {
        if (specialDaySpecification.IsFixedAndOccurrenceBased)
            return FixedAndOccurrenceBasedSpecialDaySpecificationEvaluator.Evaluate(year, specialDaySpecification);
        else if (specialDaySpecification.IsFixedWithExactDate)
            return FixedWithExactDateSpecialDaySpecificationEvaluator.Evaluate(year, specialDaySpecification);
        else if (specialDaySpecification.IsOneTimeAndOccurrenceBased)
            return OneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator.Evaluate(year, specialDaySpecification);
        else //if (specialDaySpecification.IsOneTimeWithExactDate)
            return OneTimeWithExactDateSpecialDaySpecificationEvaluator.Evaluate(year, specialDaySpecification);
    }

    public bool IsWeekend(DateOnly date)
    {
        return WeekendProvider?.IsWeekend(date) ?? false;
    }

    public bool IsWeekend(DayOfWeek dayOfWeek)
    {
        return WeekendProvider?.IsWeekend(dayOfWeek) ?? false;
    }
}
