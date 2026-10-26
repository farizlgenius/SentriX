using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Alert;
using Core.Contract.DTOs.Area;

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
            x.doors_in.Select(x => new DoorAreaDto(
                  x.guid,
                  x.antipassback_in
                  )).ToList(),
                  x.doors_out.Select(x => new DoorAreaDto(
                  x.guid,
                  x.antipassback_in
                  )).ToList(),
            x.device.guid,
            x.is_active,
            x.is_default
      );

      public static readonly Expression<Func<Persistences.Entities.Area, CommandDetailDto>> ToCommand =
    x => new CommandDetailDto(
      x.device_id,
      x.device.external_id ?? -1,
      x.device.mac,
      x.device.ip,
      -1,
      -1,
      x.device.vendor,
      x.location_id,
      x.id,
      x.external_id,
      -1
    );
}