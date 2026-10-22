using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Door;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Turnstile;

namespace Core.Infrastructure.Projections;

public static class TurnstileProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Turnstile, TurnstileDto>> ToDto = 
        x => new TurnstileDto(
        x.guid,
        x.name,
        x.lanes.Select(l => new LaneDto(
          l.guid,
          l.lane_no,
          l.readers.Select(x => new ReaderDto(
            x.slot_no,
            x.mode,
            x.metadata,
            x.vendor,
            x.reader_direction,
            x.device_module.guid
            )).ToList(),
            l.sensor == null ? null :
            new SensorDto(
              l.sensor.slot_no,
              l.sensor.metadata,
              l.sensor.vendor,
              l.sensor.device_module.guid
            ),
            x.location.guid,
            x.is_active,
            x.is_default
        )).ToList(),
        x.location.guid,
        x.location.name,
        x.is_active,
        x.is_default
      );

    public static readonly Expression<Func<Persistences.Entities.Turnstile,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}