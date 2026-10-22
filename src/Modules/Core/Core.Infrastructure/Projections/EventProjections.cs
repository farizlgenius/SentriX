using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Events.Event;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class EventProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Event, EventDto>> ToDto = 
        x => new EventDto(
        x.guid,
        x.timestamp,
        x.actor,
        x.module,
        x.event_type,
        x.image_name,
        x.mac,
        x.component_name,
        x.event_code,
        x.remarks,
        x.capture_image_name,
        x.vendor,
        x.location == null ? Guid.Empty : x.location.guid,
        x.is_active,
        x.is_default
      );

    public static readonly Expression<Func<Persistences.Entities.Event,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}