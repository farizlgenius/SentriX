using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Action;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Procedure;

namespace Core.Infrastructure.Projections;

public static class ProcedureProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Procedure, ProcedureDto>> ToDto = 
        x => new ProcedureDto(
                        x.guid,
                        x.name,
                        x.actions.Select(a => new ActionDto(
                            a.guid,
                            a.name,
                            a.device.guid,
                            a.device.name,
                            a.type,
                            a.type.ToString(),
                            a.sequence,
                            a.action_target.Select(s => 
                                s.door != null ? new ActionTargetDto(s.door.guid,s.door.name) :
                                   s.monitor != null ? new ActionTargetDto(s.monitor.guid,s.monitor.name) :
                                   s.control != null ? new ActionTargetDto(s.control.guid,s.control.name) :
                                   s.area != null ? new ActionTargetDto(s.area.guid,s.area.name) :
                                   s.user != null ? new ActionTargetDto(s.user.guid,s.user.name) :
                                   s.trigger_variable != null ? new ActionTargetDto(s.trigger_variable.guid,s.trigger_variable.name) :
                                   s.procedure != null ? new ActionTargetDto(s.procedure.guid,s.procedure.name) :
                                   s.timezone != null ? new ActionTargetDto(s.timezone.guid,s.timezone.name) :
                                   s.monitor_group != null ? new ActionTargetDto(s.monitor_group.guid,s.monitor_group.name) :
                                   s.trigger != null ? new ActionTargetDto(s.trigger.guid,s.trigger.name) :
                                   s.action != null ? new ActionTargetDto(s.action.guid,s.action.name) : 
                                   null
                            ),
                            a.action_target.Select(s => 
                                    s.door != null ? s.door.guid :
                                   s.monitor != null ? s.monitor.guid :
                                   s.control != null ? s.control.guid :
                                   s.area != null ? s.area.guid :
                                   s.user != null ? s.user.guid :
                                   s.trigger_variable != null ? s.trigger_variable.guid :
                                   s.procedure != null ? s.procedure.guid :
                                   s.timezone != null ? s.timezone.guid :
                                   s.monitor_group != null ? s.monitor_group.guid :
                                   s.trigger != null ? s.trigger.guid :
                                   s.action != null ? s.action.guid : 
                                  Guid.Empty
                            ),
                            a.metadata,
                            a.is_active,
                            a.is_default
                        )).ToArray(),
                        x.device.guid,
                        x.device.name,
                        x.is_active,
                        x.is_default
                  );

    public static readonly Expression<Func<Persistences.Entities.Procedure,CommandDetailDto>> ToCommand = 
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