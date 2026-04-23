using Haskap.DddBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;

namespace Modules.SpecialDayBase.Application.Contracts;
public interface ISpecialDayService : IUseCaseService
{
    IList<SpecialDayOutputDto> GetSpecialDaysInDateRange(GetSpecialDaysInDateRangeInputDto inputDto);
    IList<SpecialDayOutputDto> GetSpecialDaysInADay(GetSpecialDaysInDayInputDto inputDto);
}
