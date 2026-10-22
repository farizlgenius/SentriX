using System.Linq.Expressions;
using Core.Contract.DTOs;
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
                                    a.action.metadata
                              )
                        ).ToList(),
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