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
                        x.procedure_actions.Select(
                              a => new ActionDto(
                                a.action.guid,
                                a.action.name,
                                a.action.device.name,
                                a.action.device.guid,
                                a.action.type,
                                a.action.metadata,
                                a.action.location == null ? string.Empty : a.action.location.name,
                                a.action.location == null ? Guid.Empty : a.action.location.guid,
                                a.action.is_active,
                                a.action.is_default
                              )
                        ).ToList(),
                        x.location == null ? string.Empty : x.location.name,
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