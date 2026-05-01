using Haskap.DddBase.Domain.Shared.Resources;
using Haskap.DddBase.Utilities.Calendar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.SpecialDayAggregate;

namespace Modules.SpecialDayBase.Application;
public class FixedAndOccurrenceBasedSpecialDaySpecificationEvaluator(IServiceScopeFactory ServiceScopeFactory) : ISpecialDaySpecificationEvaluator
{
    public IList<SpecialDayOutputDto> Evaluate(int year, SpecialDaySpecification specialDaySpecification)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var commonTextsLocalizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<CommonTexts>>();

        var localizedEveDay = commonTextsLocalizer["EveDay"];
        var localizedDay = commonTextsLocalizer["Day"];

        List<SpecialDayOutputDto> specialDayDtos = [];

        if (specialDaySpecification.IsFixedAndOccurrenceBased)
        {
            var date = CalendarHelper.FindOccurrenceOfDayOfWeek(year, specialDaySpecification.Month, specialDaySpecification.DayOfWeek!.Value, (int)specialDaySpecification.Occurrence!.Value);

            if (specialDaySpecification.HasEveDay)
            {
                var eveDay = date!.Value.AddDays(-1);

                if (eveDay.Year == year)
                {
                    specialDayDtos.Add(new SpecialDayOutputDto
                    {
                        Date = eveDay,
                        Group = specialDaySpecification.Group,
                        IsHoliday = specialDaySpecification.IsHoliday,
                        Name = specialDaySpecification.GetLocalizedName().Value + " " + localizedEveDay, //specialDaySpecification.Name + " Eve"
                        IsEveDay = true,
                        EveDayDuration = specialDaySpecification.EveDayDuration
                    });
                }
                else
                {
                    var nextDate = CalendarHelper.FindOccurrenceOfDayOfWeek(year + 1, specialDaySpecification.Month, specialDaySpecification.DayOfWeek!.Value, (int)specialDaySpecification.Occurrence!.Value);
                    var nextEveDay = nextDate!.Value.AddDays(-1);

                    if (nextEveDay.Year == year)
                    {
                        specialDayDtos.Add(new SpecialDayOutputDto
                        {
                            Date = nextEveDay,
                            Group = specialDaySpecification.Group,
                            IsHoliday = specialDaySpecification.IsHoliday,
                            Name = specialDaySpecification.GetLocalizedName().Value + " " + localizedEveDay,
                            IsEveDay = true,
                            EveDayDuration = specialDaySpecification.EveDayDuration
                        });
                    }
                }
            }

            if (specialDaySpecification.LengthInDays == 1)
            {
                specialDayDtos.Add(new SpecialDayOutputDto
                {
                    Date = date!.Value,
                    Group = specialDaySpecification.Group,
                    IsHoliday = specialDaySpecification.IsHoliday,
                    Name = specialDaySpecification.GetLocalizedName().Value
                });
            }
            else
            {
                for (var i = 0; i < specialDaySpecification.LengthInDays; i++)
                {
                    var day = date!.Value.AddDays(i);

                    if (day.Year != year)
                    {
                        break;
                    }

                    specialDayDtos.Add(new SpecialDayOutputDto
                    {
                        Date = day,
                        Group = specialDaySpecification.Group,
                        IsHoliday = specialDaySpecification.IsHoliday,
                        Name = string.Format("{0} {1}. {2}", specialDaySpecification.GetLocalizedName().Value, i + 1, localizedDay),
                    });
                }
            }
        }

        return specialDayDtos;
    }
}
