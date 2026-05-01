using Haskap.DddBase.Domain.Shared.Enums;
using Modules.SpecialDayBase.Application.Dtos;

namespace Modules.SpecialDayBase.Domain.Providers;

public interface ISpecialDayCalendarProvider
{
    Country ForCountry { get; }
    IWeekendProvider? WeekendProvider { get; }
    IList<SpecialDayOutputDto> GetSpecialDays(int year);
    IList<SpecialDayOutputDto> GetSpecialDays(DateOnly startDate, DateOnly endDate);
    bool IsSpecialDay(DateOnly date);
    bool TryGetSpecialDays(DateOnly day, out List<SpecialDayOutputDto> specialDays);
    string GetLocalizedGroupName(string group);
    bool IsWeekend(DateOnly date);
    bool IsWeekend(DayOfWeek dayOfWeek);
    IList<LongWeekendDayOutputDto> GetLongWeekendDays(DateOnly startDate, DateOnly endDate);
}