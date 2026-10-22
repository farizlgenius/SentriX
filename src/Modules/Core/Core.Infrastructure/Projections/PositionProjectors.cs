using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Position;

namespace Core.Infrastructure.Projections;

public static class PositionProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Position, PositionDto>> ToDto = 
        x => new PositionDto(
                            x.guid,
                            x.name,
                            x.description,
                            x.department.guid,
                            x.department.name,
                            x.department.company.guid,
                            x.department.company.name,
                            x.is_active,
                            x.is_default
                          );

    public static readonly Expression<Func<Persistences.Entities.Position,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}