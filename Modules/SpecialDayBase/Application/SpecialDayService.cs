using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application.Dtos;
using Modules.SpecialDayBase.Domain.Providers;

namespace Modules.SpecialDayBase.Application;
public class SpecialDayService : UseCaseService, ISpecialDayService
{
    private readonly IServiceProvider _serviceProvider;

    public SpecialDayService(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public IList<SpecialDayOutputDto> GetSpecialDaysInDateRange(GetSpecialDaysInDateRangeInputDto inputDto)
    {
        var startDate = DateOnly.FromDateTime(inputDto.StartDateTime);
        var endDate = DateOnly.FromDateTime(inputDto.EndDateTime);

        if (inputDto.ForCountries.Contains(Country.All))
        {
            return _serviceProvider.GetServices<ISpecialDayCalendarProvider>()
                .SelectMany(x => x.GetSpecialDays(startDate, endDate))
                .ToList();
        }

        return _serviceProvider.GetServices<ISpecialDayCalendarProvider>()
            .SelectMany(x =>
            {
                if (inputDto.ForCountries.Contains(x.ForCountry))
                {
                    return x.GetSpecialDays(startDate, endDate);
                }

                return [];
            })
            .ToList();
    }

    public IList<SpecialDayOutputDto> GetSpecialDaysInADay(GetSpecialDaysInDayInputDto inputDto)
    {
        var day = DateOnly.FromDateTime(inputDto.DayDateTime);

        if (inputDto.ForCountries.Contains(Country.All))
        {
            return _serviceProvider.GetServices<ISpecialDayCalendarProvider>()
                .SelectMany(x =>
                {
                    x.TryGetSpecialDays(day, out var specialDays);
                    return specialDays;
                })
                .ToList();
        }

        return _serviceProvider.GetServices<ISpecialDayCalendarProvider>()
            .SelectMany(x =>
            {
                if (inputDto.ForCountries.Contains(x.ForCountry))
                {
                    x.TryGetSpecialDays(day, out var specialDays);
                    return specialDays;
                }

                return [];
            })
            .ToList();
    }

    public bool IsWeekend(DateTime dayDateTime, IEnumerable<Country> forCountries)
    {
        return IsWeekend(dayDateTime.DayOfWeek, forCountries);
    }

    public bool IsWeekend(DayOfWeek dayOfWeek, IEnumerable<Country> forCountries)
    {
        IEnumerable<ISpecialDayCalendarProvider> specialDayCalendarProviders = [];

        if (forCountries.Contains(Country.All))
        {
            specialDayCalendarProviders = _serviceProvider.GetServices<ISpecialDayCalendarProvider>();
        }
        else
        {
            specialDayCalendarProviders = _serviceProvider.GetServices<ISpecialDayCalendarProvider>()
                .Where(x => forCountries.Contains(x.ForCountry));
        }

        foreach (var specialDayCalendarProvider in specialDayCalendarProviders)
        {
            if (specialDayCalendarProvider.IsWeekend(dayOfWeek))
            {
                return true;
            }
        }

        return false;
    }
}