using Modules.SpecialDayBase.Domain.Shared.Enums;

namespace Modules.SpecialDayBase.Application.Dtos;
public class LongWeekendDayOutputDto
{
    public DateOnly Date { get; set; }
    public bool IsHoliday { get; set; }
    public bool IsWeekend { get; set; }
    public IList<string> DisplayTexts { get; set; }
    public LongWeekendDayWorkDuration WorkDuration { get; set; }
}
