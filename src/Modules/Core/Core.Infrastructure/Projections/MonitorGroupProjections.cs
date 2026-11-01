using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Location;
using Core.Contract.DTOs.MonitorGroup;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class MonitorGroupProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.MonitorGroup, MonitorGroupDto>> ToDto = 
        x => new MonitorGroupDto(
            x.guid,
            x.name,
            x.components.Select(c => new MonitorGroupComponentDto(
                  c.type == SharedKernel.Enums.MonitorType.MONITOR_POINT ? c.input == null ? Guid.Empty : c.input.guid : c.type == SharedKernel.Enums.MonitorType.FORCED_OPEN ? c.door == null ? Guid.Empty : c.door.guid : c.door == null ? Guid.Empty : c.door.guid,
                  c.type
            )).ToList(),
            x.device.guid,
            x.is_active,
            x.is_default
                  );

    public static readonly Expression<Func<Persistences.Entities.MonitorGroup,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
      x.device.id,
      x.device.external_id ?? -1,
      x.device.mac,
      x.device.ip,
      -1,
      -1,
      x.device.vendor,
      x.device.location_id,
      x.id,
      x.external_id,
      x.altr_external_id
    );
}