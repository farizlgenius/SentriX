using System.Linq.Expressions;
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
}