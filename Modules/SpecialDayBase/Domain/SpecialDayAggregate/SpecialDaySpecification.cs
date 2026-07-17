using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Utilities.Guids;
using Modules.SpecialDayBase.Domain.Shared.Enums;

namespace Modules.SpecialDayBase.Domain.SpecialDayAggregate;

[AddAuditHistoryLog]
public class SpecialDaySpecification : AggregateRoot, IHasMultiTenant
{
    public int? Year { get; private set; } = null;
    public int Month { get; private set; }
    public int? Day { get; private set; } = null;
    public DayOfWeek? DayOfWeek { get; private set; } = null;
    public SpecialDayOccurrenceInAMonth? Occurrence { get; private set; } = null;
    public IReadOnlyList<Name> Names => _names.AsReadOnly();
    private List<Name> _names = [];
    public bool HasEveDay { get; private set; }
    public EveDayDuration EveDayDuration { get; private set; }
    public int LengthInDays { get; private set; }
    public bool IsFixed { get; private set; }
    public bool IsOccurrenceBased { get; private set; }
    public bool IsHoliday { get; private set; }
    public string Group { get; private set; }
    public int? YearBelongsTo { get; private set; }
    public bool UseHijriCalendar { get; private set; }
    public Guid? TenantId { get; set; }


    public bool IsFixedAndOccurrenceBased => IsFixed && IsOccurrenceBased;
    public bool IsFixedWithExactDate => IsFixed && !IsOccurrenceBased;
    public bool IsOneTimeAndOccurrenceBased => !IsFixed && IsOccurrenceBased;
    public bool IsOneTimeWithExactDate => !IsFixed && !IsOccurrenceBased;


    private SpecialDaySpecification()
    {
    }

    private void SetNames(IList<Name> names)
    {
        Guard.Against.NullOrEmpty(names, nameof(names));

        foreach (var name in names)
        {
            if (_names.Contains(name))
            {
                continue;
            }

            _names.Add(name);
        }
    }

    private void SetLength(int lengthInDays)
    {
        Guard.Against.NegativeOrZero(lengthInDays);

        LengthInDays = lengthInDays;
    }

    private void SetEveDayPreferences(bool hasEveDay, EveDayDuration eveDayDuration)
    {
        if (hasEveDay && eveDayDuration == EveDayDuration.None)
        {
            throw new ArgumentException("Eve day type must be specified if hasEveDay is true.");
        }

        if (!hasEveDay && eveDayDuration != EveDayDuration.None)
        {
            throw new ArgumentException("Eve day type must be None if hasEveDay is false.");
        }

        HasEveDay = hasEveDay;
        EveDayDuration = eveDayDuration;
    }

    public static SpecialDaySpecification CreateFixedWithExactDateSpecialDay(int month, int day, bool isHoliday, string group, bool useHijriCalendar, IList<Name> names, bool hasEveDay = false, EveDayDuration eveDayDuration = EveDayDuration.None, int lengthInDays = 1)
    {
        var specialDaySpecification = new SpecialDaySpecification
        {
            Id = GuidGenerator.CreateSimpleGuid(),
            Year = null,
            Month = month,
            Day = day,
            DayOfWeek = null,
            Occurrence = null,
            IsHoliday = isHoliday,
            Group = group,
            YearBelongsTo = null,
            IsFixed = true,
            IsOccurrenceBased = false,
            UseHijriCalendar = useHijriCalendar
        };

        specialDaySpecification.SetNames(names);
        specialDaySpecification.SetLength(lengthInDays);
        specialDaySpecification.SetEveDayPreferences(hasEveDay, eveDayDuration);

        return specialDaySpecification;
    }

    public static SpecialDaySpecification CreateFixedAndOccurrenceBasedSpecialDay(int month, DayOfWeek dayOfWeek, SpecialDayOccurrenceInAMonth occurrence, bool isHoliday, string group, IList<Name> names, bool hasEveDay = false, EveDayDuration eveDayDuration = EveDayDuration.None, int lengthInDays = 1)
    {
        var specialDaySpecification = new SpecialDaySpecification
        {
            Id = GuidGenerator.CreateSimpleGuid(),
            Year = null,
            Month = month,
            Day = null,
            DayOfWeek = dayOfWeek,
            Occurrence = occurrence,
            IsHoliday = isHoliday,
            Group = group,
            YearBelongsTo = null,
            IsFixed = true,
            IsOccurrenceBased = true,
            UseHijriCalendar = false
        };

        specialDaySpecification.SetNames(names);
        specialDaySpecification.SetLength(lengthInDays);
        specialDaySpecification.SetEveDayPreferences(hasEveDay, eveDayDuration);

        return specialDaySpecification;
    }

    public static SpecialDaySpecification CreateOneTimeWithExactDateSpecialDay(int year, int month, int day, bool isHoliday, string group, int? yearBelongsTo, bool useHijriCalendar, IList<Name> names, bool hasEveDay = false, EveDayDuration eveDayDuration = EveDayDuration.None, int lengthInDays = 1)
    {
        var specialDaySpecification = new SpecialDaySpecification
        {
            Id = GuidGenerator.CreateSimpleGuid(),
            Year = year,
            Month = month,
            Day = day,
            DayOfWeek = null,
            Occurrence = null,
            IsHoliday = isHoliday,
            Group = group,
            YearBelongsTo = yearBelongsTo,
            IsFixed = false,
            IsOccurrenceBased = false,
            UseHijriCalendar = useHijriCalendar
        };

        specialDaySpecification.SetNames(names);
        specialDaySpecification.SetLength(lengthInDays);
        specialDaySpecification.SetEveDayPreferences(hasEveDay, eveDayDuration);

        return specialDaySpecification;
    }

    public static SpecialDaySpecification CreateOneTimeAndOccurrenceBasedSpecialDay(int year, int month, DayOfWeek dayOfWeek, SpecialDayOccurrenceInAMonth occurrence, bool isHoliday, string group, int? yearBelongsTo, IList<Name> names, bool hasEveDay = false, EveDayDuration eveDayDuration = EveDayDuration.None, int lengthInDays = 1)
    {
        var specialDaySpecification = new SpecialDaySpecification
        {
            Id = GuidGenerator.CreateSimpleGuid(),
            Year = year,
            Month = month,
            Day = null,
            DayOfWeek = dayOfWeek,
            Occurrence = occurrence,
            IsHoliday = isHoliday,
            Group = group,
            YearBelongsTo = yearBelongsTo,
            IsFixed = false,
            IsOccurrenceBased = true,
            UseHijriCalendar = false
        };

        specialDaySpecification.SetNames(names);
        specialDaySpecification.SetLength(lengthInDays);
        specialDaySpecification.SetEveDayPreferences(hasEveDay, eveDayDuration);

        return specialDaySpecification;
    }

    public Name GetLocalizedName()
    {
        return Names.Where(x => x.Locale == Locale.CurrentUiLocale).FirstOrDefault()
            ?? Names.Where(x => x.Locale == Locale.Default).FirstOrDefault()
            ?? Names.First();
    }
}
