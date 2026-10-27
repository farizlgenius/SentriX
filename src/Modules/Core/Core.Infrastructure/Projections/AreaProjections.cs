using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Alert;
using Core.Contract.DTOs.Area;
using SharedKernel.Enums;

namespace Core.Infrastructure.Projections;

public static class AreaProjections
{
      public static readonly Expression<Func<Persistences.Entities.Area, AreaDto>> ToDto =
      x => new AreaDto(
            x.guid,
            x.name,
            x.multi_occ_mode,
            x.area_mode,
            x.occ_max,
            x.occ_down,
            x.occ_up,
            x.metadata,
            x.doors_from.Select(x => new DoorAreaDto(x.guid,x.antipassback_from,x.from_area == null ? Guid.Empty : x.from_area.guid,x.antipassback_to,x.to_area == null ? Guid.Empty : x.to_area.guid,x.type)).ToList(),
            x.location == null ? Guid.Empty :x.location.guid,
            x.is_active,
            x.is_default
      );

      public static readonly Expression<Func<Persistences.Entities.Area, CommandDetailDto>> ToCommand =
    x => new CommandDetailDto();
}