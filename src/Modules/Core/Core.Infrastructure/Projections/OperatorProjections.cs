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
        x.role.guid,
        x.role.name,
        x.joined_date,
        x.expired_date ?? new DateTime(9999, 01, 01, 0, 0, 0, DateTimeKind.Utc),
        x.operator_locations.Select(x => x.location.guid).ToList(),
        x.is_active,
        x.is_default
      );

    public static readonly Expression<Func<Persistences.Entities.Operator,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto();
}