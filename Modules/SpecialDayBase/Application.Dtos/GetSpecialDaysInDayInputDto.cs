using Haskap.DddBase.Domain.Shared.Enums;

namespace Modules.SpecialDayBase.Application.Dtos;
public class GetSpecialDaysInDayInputDto
{
    public DateTime DayDateTime { get; set; }
    public IList<Country> ForCountries { get; set; } = [Country.All];
}
