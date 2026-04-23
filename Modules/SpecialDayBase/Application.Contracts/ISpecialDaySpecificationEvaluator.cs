using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.SpecialDayAggregate;

namespace Modules.SpecialDayBase.Application.Contracts;
public interface ISpecialDaySpecificationEvaluator
{
    IList<SpecialDayOutputDto> Evaluate(int year, SpecialDaySpecification specialDaySpecification);
}
