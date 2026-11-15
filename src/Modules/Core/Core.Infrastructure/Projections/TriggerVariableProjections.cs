using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Action;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Procedure;
using Core.Contract.DTOs.Trigger;

namespace Core.Infrastructure.Projections;

public static class TriggerVariableProjections
{

    public static readonly Expression<Func<Persistences.Entities.TriggerVariable, TriggerVariableDto>> ToDto = 
        x => new TriggerVariableDto(
            x.guid,
            new BaseDto(
                  x.trigger.guid,
                  x.trigger.name
            ),
            new BaseDto(
                  x.device.guid,
                  x.device.name    
            ),
            x.is_active,
            x.is_default
            );

    public static readonly Expression<Func<Persistences.Entities.TriggerVariable,CommandDetailDto>> ToCommand = 
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