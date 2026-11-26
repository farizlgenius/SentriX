using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class OperatorProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Operator, OperatorDto>> ToDto = 
        x => new OperatorDto(
        x.guid,
        x.username,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.email,
        x.phone,
        x.joined_date,
        x.expired_date ?? new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        new BaseDto(
          x.role.guid,
          x.role.name
        ),
        x.operator_locations.Select(l => new BaseDto(
          l.location.guid,
          l.location.name
        )),
        x.is_active,
        x.is_default
      );

    public static readonly Expression<Func<Persistences.Entities.Operator,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}