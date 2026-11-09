using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Action;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Procedure;
using Core.Contract.DTOs.Trigger;

namespace Core.Infrastructure.Projections;

public static class TriggerProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Trigger, TriggerDto>> ToDto = 
        x => new TriggerDto(
            x.guid,
            x.name
            );

    public static readonly Expression<Func<Persistences.Entities.Trigger,CommandDetailDto>> ToCommand = 
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