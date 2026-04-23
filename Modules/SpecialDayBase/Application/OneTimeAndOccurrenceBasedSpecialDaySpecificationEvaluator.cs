using Haskap.DddBase.Domain.Shared.Resources;
using Haskap.DddBase.Utilities.Calendar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.SpecialDayAggregate;

namespace Modules.SpecialDayBase.Application;
public class OneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator(IServiceScopeFactory ServiceScopeFactory) : ISpecialDaySpecificationEvaluator
{
    public IList<SpecialDayOutputDto> Evaluate(int year, SpecialDaySpecification specialDaySpecification)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var commonTextsLocalizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<CommonTexts>>();

        var localizedEveDay = commonTextsLocalizer["EveDay"];
        var localizedDay = commonTextsLocalizer["Day"];

        List<SpecialDayOutputDto> specialDayDtos = [];

        if (specialDaySpecification.IsOneTimeAndOccurrenceBased)
        {
            if (specialDaySpecification.Year == year)
            {
                var date = CalendarHelper.FindOccurrenceOfDayOfWeek(specialDaySpecification.Year!.Value, specialDaySpecification.Month, specialDaySpecification.DayOfWeek!.Value, (int)specialDaySpecification.Occurrence!.Value);

                if (specialDaySpecification.HasEveDay)
                {
                    var eveDay = date.AddDays(-1);

                    if (eveDay.Year == year)
                    {
                        specialDayDtos.Add(new SpecialDayOutputDto
                        {
                            Date = eveDay,
                            Group = specialDaySpecification.Group,
                            IsHoliday = specialDaySpecification.IsHoliday,
                            Name = specialDaySpecification.GetLocalizedName().Value + " " + localizedEveDay,
                            IsEveDay = true,
                            EveDayType = specialDaySpecification.EveDayType
                        });
                    }
                }

                if (specialDaySpecification.LengthInDays == 1)
                {
                    specialDayDtos.Add(new SpecialDayOutputDto
                    {
                        Date = date,
                        Group = specialDaySpecification.Group,
                        IsHoliday = specialDaySpecification.IsHoliday,
                        Name = specialDaySpecification.GetLocalizedName().Value
                    });
                }
                else
                {
                    for (var i = 0; i < specialDaySpecification.LengthInDays; i++)
                    {
                        var day = date.AddDays(i);

                        if (day.Year != year)
                        {
                            break;
                        }

                        specialDayDtos.Add(new SpecialDayOutputDto
                        {
                            Date = day,
                            Group = specialDaySpecification.Group,
                            IsHoliday = specialDaySpecification.IsHoliday,
                            Name = string.Format("{0} {1}. {2}", specialDaySpecification.GetLocalizedName().Value, i + 1, localizedDay)
                        });
                    }
                }
            }
        }

        return specialDayDtos;
    }
}
