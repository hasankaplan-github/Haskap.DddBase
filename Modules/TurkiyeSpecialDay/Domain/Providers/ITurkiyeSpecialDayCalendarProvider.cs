using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.Providers;

namespace Modules.TurkiyeSpecialDay.Domain.Providers;

public interface ITurkiyeSpecialDayCalendarProvider : ISpecialDayCalendarProvider
{
    SpecialDayOutputDto NewYear(int year);
    SpecialDayOutputDto NationalSovereigntyAndChildrensDay(int year);
    SpecialDayOutputDto? LabourDay(int year);
    IEnumerable<SpecialDayOutputDto> Ramadan(int year);
    SpecialDayOutputDto YouthAndSportsDay(int year);
    IEnumerable<SpecialDayOutputDto> FeastOfSacrifices(int year);
    SpecialDayOutputDto? DemocracyAndNationalUnityDay(int year);
    SpecialDayOutputDto VictoryDay(int year);
    IEnumerable<SpecialDayOutputDto> RepublicDay(int year);
    SpecialDayOutputDto ValentinesDay(int year);
    SpecialDayOutputDto MothersDay(int year);
    SpecialDayOutputDto FathersDay(int year);
    SpecialDayOutputDto TheCommemorationDayOfAtatürk(int year);
}