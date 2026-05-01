using Modules.SpecialDayBase.Domain.Shared.Enums;

namespace Modules.SpecialDayBase.Application.Dtos;
public class SpecialDayOutputDto
{
    public DateOnly Date { get; set; }
    public bool IsHoliday { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public string DisplayName { get; set; }
    public bool IsEveDay { get; set; } = false;
    public EveDayDuration EveDayDuration { get; set; } = EveDayDuration.None;
}
