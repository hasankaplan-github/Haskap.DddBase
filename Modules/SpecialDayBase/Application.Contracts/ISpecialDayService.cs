using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Domain.Shared.Enums;
using Modules.SpecialDayBase.Application.Dtos;

namespace Modules.SpecialDayBase.Application.Contracts;
public interface ISpecialDayService : IUseCaseService
{
    IList<SpecialDayOutputDto> GetSpecialDaysInDateRange(GetSpecialDaysInDateRangeInputDto inputDto);
    IList<SpecialDayOutputDto> GetSpecialDaysInADay(GetSpecialDaysInDayInputDto inputDto);
    bool IsWeekend(DateTime dayDateTime, IEnumerable<Country> forCountries);
    bool IsWeekend(DayOfWeek dayOfWeek, IEnumerable<Country> forCountries);
}
