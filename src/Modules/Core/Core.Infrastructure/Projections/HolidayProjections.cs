using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Time;

namespace Core.Infrastructure.Projections;

public static class HolidayProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Holiday, HolidayDto>> ToDto = 
        x => new HolidayDto(
          x.guid,
          x.name,
          x.start,
          x.end,
          x.is_active,
          x.is_default,
          x.location.guid
        );

    public static readonly Expression<Func<Persistences.Entities.Holiday,CommandDetailDto>> ToCommand = 
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