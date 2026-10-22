using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Time;

namespace Core.Infrastructure.Projections;

public static class TimeProjections
{
      // Reusable projection expression for EF Core queries
      public static readonly Expression<Func<Persistences.Entities.TimeZone, TimeZoneDto>> ToDto =
          x => new TimeZoneDto(
          x.guid,
          x.name,
          x.timezone_intervals.Select(
            x => new IntervalDto(
              x.interval.guid,
              x.interval.day == null ?
              new DayInWeekDto() :
              new DayInWeekDto(
                x.interval.day.sunday,
                x.interval.day.monday,
                x.interval.day.tuesday,
                x.interval.day.wednesday,
                x.interval.day.thursday,
                x.interval.day.friday,
                x.interval.day.saturday
              ),
              x.interval.start_time,
              x.interval.end_time,
              x.interval.is_active,
              x.interval.is_default
            )
          ).ToList(),
          x.location.guid,
          x.is_active,
          x.is_default
        );

      public static readonly Expression<Func<Persistences.Entities.TimeZone, CommandDetailDto>> ToCommand =
      x => new CommandDetailDto(
        -1,
        -1,
        "",
        "",
        -1,
        -1,
        SharedKernel.Enums.Vendor.unknown,
          x.location_id,
          x.id,
          x.external_id
      );
}