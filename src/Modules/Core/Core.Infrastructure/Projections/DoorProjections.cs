using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Door;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class DoorProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Door, DoorDto>> ToDto = 
        x => new DoorDto(
  x.guid,
  x.name,
  x.vendor,
  x.type,
   x.device.guid,
  x.device.name,
  x.metadata,
  x.readers.Select(
    r => new ReaderDto(
      r.slot_no,
      r.mode,
      r.metadata,
      r.vendor,
      r.reader_direction,
      r.device_module.guid
    )
  ).ToList(),
  x.buzzer == null ? null : new BuzzerDto(
    x.buzzer.slot_no,
    x.buzzer.metadata,
    x.buzzer.vendor,
    x.buzzer.device_module.guid
  ),
  x.rexes.Select(x => new RexDto(
    x.slot_no,
    x.metadata,
    x.vendor,
    x.device_module.guid,
     x.timezone == null ? default : x.timezone.guid
  )).ToList(),
  x.sensor == null ? null : new SensorDto(
    x.sensor.slot_no,
    x.sensor.metadata,
    x.sensor.vendor,
    x.sensor.device_module.guid
  ),
  x.relay == null ? null : new RelayDto(
    x.relay.slot_no,
    x.relay.metadata,
    x.relay.vendor,
    x.relay.device_module.guid
  ),
  x.follower_relay == null ? null : new RelayDto(
    x.follower_relay.slot_no,
    x.follower_relay.metadata,
    x.follower_relay.vendor,
    x.follower_relay.device_module.guid
  ),
   x.bg == null ? null : new BGDto(
    x.bg.slot_no,
    x.metadata,
    x.bg.vendor,
    x.bg.device_module.guid
  ),
  x.extended_feature_type,
  x.interior_push_button == null ? null : new InteriorPushButtonDto(
    x.interior_push_button.slot_no,
    x.interior_push_button.vendor,
    x.interior_push_button.device_module.guid,
    x.interior_push_button.metadata
  ),
  x.interior_push_button_output == null ? null : new InteriorPushButtonOutputDto(
    x.interior_push_button_output.slot_no,
    x.interior_push_button_output.vendor,
    x.interior_push_button_output.device_module.guid,
    x.interior_push_button_output.metadata
  ),
   x.location.guid,
  x.location.name,
  x.is_active,
  x.is_default
);

    public static readonly Expression<Func<Persistences.Entities.Door,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        x.device.id,
        x.device.external_id ?? -1,
        x.device.mac,
        x.device.ip,
        -1,
        -1,
        x.vendor,
        x.location_id,
        x.id,
        x.external_id,
        x.altr_external_id
    );
}