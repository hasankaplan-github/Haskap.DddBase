using Haskap.DddBase.Domain.Shared.Enums;

namespace Modules.SpecialDayBase.Application.Dtos;
public class GetSpecialDaysInDateRangeInputDto
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public IList<Country> ForCountries { get; set; } = [Country.All];
}
