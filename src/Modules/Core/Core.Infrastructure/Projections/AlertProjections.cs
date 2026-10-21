using System.Linq.Expressions;
using Core.Contract.DTOs.Alert;

namespace Core.Infrastructure.Projections;

public static class AlertProjections
{
      public static readonly Expression<Func<Persistences.Entities.Alert,AlertDto>> ToDto = 
      x => new AlertDto(
            x.guid,
            x.name,
            x.advice,
            x.location == null ? Guid.Empty : x.location.guid,
            x.location == null ? string.Empty : x.location.name,
            x.alert_level.guid,
            x.alert_level.color_code,
            x.alert_level.name,
            x.core_event.guid,
            x.core_event.guid,
            x.alert_components.Select(x => x.door.guid).ToList(),
            x.is_active,
            x.is_default
      );
}