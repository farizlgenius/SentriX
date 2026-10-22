using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Action;

namespace Core.Infrastructure.Projections;

public static class ActionProjections
{
      public static readonly Expression<Func<Persistences.Entities.Action,ActionDto>> ToDto = 
      x => new ActionDto(
            x.guid,
            x.name,
            x.device.guid,
            x.type,
            x.metadata,
            x.is_active,
            x.is_default
      );

      public static readonly Expression<Func<Persistences.Entities.Action,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        x.device.id,
        x.device.external_id ?? -1,
        x.device.mac,
        x.device.ip,
        -1,
        -1,
        x.device.vendor,
        x.location_id,
        x.id,
        x.external_id
    );
}