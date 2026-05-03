using Haskap.DddBase.Domain.Shared.Enums;
using Microsoft.Extensions.Localization;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.Providers;
using Modules.SpecialDayBase.Domain.Shared.Consts;
using Modules.SpecialDayBase.Domain.Shared.Enums;
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

    public virtual IList<LongWeekendDayOutputDto> GetLongWeekendDays(DateOnly startDate, DateOnly endDate)
    {
        if (WeekendProvider is null)
        {
            return [];
        }

        var holidays = GetSpecialDays(startDate, endDate)
            .Where(x => x.IsHoliday)
            .OrderBy(x => x.Date)
            .ToList();

        var longWeekendDays = new List<LongWeekendDayOutputDto>();

        foreach (var holiday in holidays)
        {
            if (IsWeekend(holiday.Date))
            {
                continue;
            }

            var longWeekendDayWorkDuration = holiday.IsEveDay && holiday.EveDayDuration == EveDayDuration.HalfDay
                ? LongWeekendDayWorkDuration.HalfWorkDay
                : LongWeekendDayWorkDuration.None;

            var longWeekendDayDisplayTexts = holidays.Where(x => x.Date == holiday.Date)
                .Select(x => x.DisplayName)
                .ToList();

            if(longWeekendDayWorkDuration == LongWeekendDayWorkDuration.HalfWorkDay)
            {
                longWeekendDayDisplayTexts.Add(Localizer["HalfWorkDay"]);
            }

            var existingLongWeekendDay = longWeekendDays.FirstOrDefault(x => x.Date == holiday.Date);

            if (existingLongWeekendDay is not null)
            {
                if ((existingLongWeekendDay.WorkDuration == LongWeekendDayWorkDuration.HalfWorkDay && longWeekendDayWorkDuration == LongWeekendDayWorkDuration.None) ||
                    (existingLongWeekendDay.WorkDuration == LongWeekendDayWorkDuration.FullWorkDay && (longWeekendDayWorkDuration == LongWeekendDayWorkDuration.HalfWorkDay || longWeekendDayWorkDuration == LongWeekendDayWorkDuration.None)))
                {
                    existingLongWeekendDay.IsHoliday = true;
                    existingLongWeekendDay.WorkDuration = longWeekendDayWorkDuration;
                    existingLongWeekendDay.DisplayTexts = longWeekendDayDisplayTexts;
                }

                continue;
            }


            longWeekendDays.Add(new LongWeekendDayOutputDto
            {
                Date = holiday.Date,
                IsHoliday = true,
                IsWeekend = false,
                WorkDuration = longWeekendDayWorkDuration,
                DisplayTexts = longWeekendDayDisplayTexts
            });

            var longWeekendDaysBefore = GetLongWeekendDaysBefore(holiday, holidays);
            foreach (var longWeekendDayBefore in longWeekendDaysBefore)
            {
                var existingLongWeekendDayBefore = longWeekendDays.FirstOrDefault(x => x.Date == longWeekendDayBefore.Date);
                if (existingLongWeekendDayBefore is not null)
                {
                    if ((existingLongWeekendDayBefore.WorkDuration == LongWeekendDayWorkDuration.HalfWorkDay && longWeekendDayBefore.WorkDuration == LongWeekendDayWorkDuration.None) ||
                        (existingLongWeekendDayBefore.WorkDuration == LongWeekendDayWorkDuration.FullWorkDay && (longWeekendDayBefore.WorkDuration == LongWeekendDayWorkDuration.HalfWorkDay || longWeekendDayBefore.WorkDuration == LongWeekendDayWorkDuration.None)))
                    {
                        existingLongWeekendDayBefore.IsHoliday = longWeekendDayBefore.IsHoliday;
                        existingLongWeekendDayBefore.WorkDuration = longWeekendDayBefore.WorkDuration;
                        existingLongWeekendDayBefore.DisplayTexts = longWeekendDayBefore.DisplayTexts;
                    }

                    continue;
                }

                longWeekendDays.Add(longWeekendDayBefore);
            }

            var longWeekendDaysAfter = GetLongWeekendDaysAfter(holiday, holidays);
            foreach (var longWeekendDayAfter in longWeekendDaysAfter)
            {
                var existingLongWeekendDayAfter = longWeekendDays.FirstOrDefault(x => x.Date == longWeekendDayAfter.Date);
                if (existingLongWeekendDayAfter is not null)
                {
                    if ((existingLongWeekendDayAfter.WorkDuration == LongWeekendDayWorkDuration.HalfWorkDay && longWeekendDayAfter.WorkDuration == LongWeekendDayWorkDuration.None) ||
                        (existingLongWeekendDayAfter.WorkDuration == LongWeekendDayWorkDuration.FullWorkDay && (longWeekendDayAfter.WorkDuration == LongWeekendDayWorkDuration.HalfWorkDay || longWeekendDayAfter.WorkDuration == LongWeekendDayWorkDuration.None)))
                    {
                        existingLongWeekendDayAfter.IsHoliday = longWeekendDayAfter.IsHoliday;
                        existingLongWeekendDayAfter.WorkDuration = longWeekendDayAfter.WorkDuration;
                        existingLongWeekendDayAfter.DisplayTexts = longWeekendDayAfter.DisplayTexts;
                    }

                    continue;
                }

                longWeekendDays.Add(longWeekendDayAfter);
            }
        }

        longWeekendDays.RemoveAll(x => x.Date < startDate || x.Date > endDate);

        return longWeekendDays;
    }

    private List<LongWeekendDayOutputDto> GetLongWeekendDaysBefore(SpecialDayOutputDto holiday, IList<SpecialDayOutputDto> holidays)
    {
        var longWeekendDays = new List<LongWeekendDayOutputDto>();

        var previousDayCount = (int)holiday.Date.DayOfWeek - ((int)WeekendProvider!.FirstWeekendDay + 1);

        for (int i = 1; i <= previousDayCount; i++)
        {
            var previousDayDate = holiday.Date.AddDays(-i);
            var existingHolidays = holidays.Where(x => x.Date == previousDayDate).ToList();

            var isHoliday = existingHolidays.Any();

            var longWeekendDayDuration = LongWeekendDayWorkDuration.None;
            if (isHoliday && existingHolidays.All(x => x.IsEveDay && x.EveDayDuration == EveDayDuration.HalfDay))
            {
                longWeekendDayDuration = LongWeekendDayWorkDuration.HalfWorkDay;
            }
            else if (!isHoliday)
            {
                longWeekendDayDuration = LongWeekendDayWorkDuration.FullWorkDay;
            }

            var longWeekendDayDisplayTexts = existingHolidays.Select(x => x.DisplayName).ToList();
            if (longWeekendDayDuration == LongWeekendDayWorkDuration.HalfWorkDay)
            {
                longWeekendDayDisplayTexts.Add(Localizer["HalfWorkDay"]);
            }
            else if (longWeekendDayDuration == LongWeekendDayWorkDuration.FullWorkDay)
            {
                longWeekendDayDisplayTexts.Add(Localizer["WorkDay"]);
            }

            longWeekendDays.Add(new LongWeekendDayOutputDto
            {
                Date = previousDayDate,
                IsHoliday = isHoliday,
                IsWeekend = false,
                WorkDuration = longWeekendDayDuration,
                DisplayTexts = longWeekendDayDisplayTexts
            });
        }


        var previousHolidayDate = holiday.Date.AddDays(-(++previousDayCount));
        while (IsWeekend(previousHolidayDate) || holidays.Any(x => x.Date == previousHolidayDate))
        {
            var isWeekend = IsWeekend(previousHolidayDate);

            var previousHolidays = holidays.Where(x => x.Date == previousHolidayDate).ToList();

            var workDuration = LongWeekendDayWorkDuration.None;
            if (!isWeekend && previousHolidays.All(x => x.IsEveDay && x.EveDayDuration == EveDayDuration.HalfDay))
            {
                workDuration = LongWeekendDayWorkDuration.HalfWorkDay;
            }

            var displayTexts = previousHolidays.Select(x => x.DisplayName).ToList();
            if (isWeekend)
            {
                displayTexts.Add(Localizer["Weekend"]);
            }
            else if (workDuration == LongWeekendDayWorkDuration.HalfWorkDay)
            {
                displayTexts.Add(Localizer["HalfWorkDay"]);
            }

            longWeekendDays.Add(new LongWeekendDayOutputDto
            {
                Date = previousHolidayDate,
                IsHoliday = true,
                IsWeekend = isWeekend,
                WorkDuration = workDuration,
                DisplayTexts = displayTexts
            });

            previousHolidayDate = holiday.Date.AddDays(-(++previousDayCount));
        }

        return longWeekendDays;
    }

    private List<LongWeekendDayOutputDto> GetLongWeekendDaysAfter(SpecialDayOutputDto holiday, IList<SpecialDayOutputDto> holidays)
    {
        var longWeekendDays = new List<LongWeekendDayOutputDto>();

        var nextDayCount = (int)WeekendProvider!.LastWeekendDay - ((int)holiday.Date.DayOfWeek + 1);
        nextDayCount = nextDayCount < 0 ? nextDayCount + 7 : nextDayCount;

        for (int i = 1; i <= nextDayCount; i++)
        {
            var nextDayDate = holiday.Date.AddDays(i);
            var existingHolidays = holidays.Where(x => x.Date == nextDayDate).ToList();

            var isHoliday = existingHolidays.Any();

            var longWeekendDayDuration = LongWeekendDayWorkDuration.None;
            if (isHoliday && existingHolidays.All(x => x.IsEveDay && x.EveDayDuration == EveDayDuration.HalfDay))
            {
                longWeekendDayDuration = LongWeekendDayWorkDuration.HalfWorkDay;
            }
            else if (!isHoliday)
            {
                longWeekendDayDuration = LongWeekendDayWorkDuration.FullWorkDay;
            }

            var longWeekendDayDisplayTexts = existingHolidays.Select(x => x.DisplayName).ToList();
            if (longWeekendDayDuration == LongWeekendDayWorkDuration.HalfWorkDay)
            {
                longWeekendDayDisplayTexts.Add(Localizer["HalfWorkDay"]);
            }
            else if (longWeekendDayDuration == LongWeekendDayWorkDuration.FullWorkDay)
            {
                longWeekendDayDisplayTexts.Add(Localizer["WorkDay"]);
            }

            longWeekendDays.Add(new LongWeekendDayOutputDto
            {
                Date = nextDayDate,
                IsHoliday = isHoliday,
                IsWeekend = false,
                WorkDuration = longWeekendDayDuration,
                DisplayTexts = longWeekendDayDisplayTexts
            });
        }

        var nextHolidayDate = holiday.Date.AddDays(++nextDayCount);
        while (IsWeekend(nextHolidayDate) || holidays.Any(x => x.Date == nextHolidayDate))
        {
            var isWeekend = IsWeekend(nextHolidayDate);

            var nextHolidays = holidays.Where(x => x.Date == nextHolidayDate).ToList();

            var workDuration = LongWeekendDayWorkDuration.None;
            if (!isWeekend && nextHolidays.All(x => x.IsEveDay && x.EveDayDuration == EveDayDuration.HalfDay))
            {
                workDuration = LongWeekendDayWorkDuration.HalfWorkDay;
            }

            var displayTexts = nextHolidays.Select(x => x.DisplayName).ToList();
            if (isWeekend)
            {
                displayTexts.Add(Localizer["Weekend"]);
            }
            else if (workDuration == LongWeekendDayWorkDuration.HalfWorkDay)
            {
                displayTexts.Add(Localizer["HalfWorkDay"]);
            }

            longWeekendDays.Add(new LongWeekendDayOutputDto
            {
                Date = nextHolidayDate,
                IsHoliday = true,
                IsWeekend = isWeekend,
                WorkDuration = workDuration,
                DisplayTexts = displayTexts
            });
            nextHolidayDate = holiday.Date.AddDays(++nextDayCount);
        }

        return longWeekendDays;
    }
}
