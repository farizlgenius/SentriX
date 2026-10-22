using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Time;

namespace Core.Infrastructure.Projections;

public static class IntervalProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Interval, IntervalDto>> ToDto = 
        x => new IntervalDto(
          x.guid,
          x.day == null ?
            new DayInWeekDto() :
            new DayInWeekDto(
              x.day.sunday,
              x.day.monday,
              x.day.tuesday,
              x.day.wednesday,
              x.day.thursday,
              x.day.friday,
              x.day.saturday
            ),
          x.start_time,
          x.end_time,
          x.is_active,
          x.is_default
        );

    public static readonly Expression<Func<Persistences.Entities.Interval,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}