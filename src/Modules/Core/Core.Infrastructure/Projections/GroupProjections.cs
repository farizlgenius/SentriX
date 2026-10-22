using System.Linq.Expressions;
using Core.Contract.Dtos.Group;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Group;
using Core.Contract.DTOs.Output;
using SharedKernel.Enums;

namespace Core.Infrastructure.Projections;

public static class GroupProjections
{
      // Reusable projection expression for EF Core queries
      public static readonly Expression<Func<Persistences.Entities.Group, GroupDto>> ToDto =
          x => new GroupDto(
                          x.guid,
                          x.name,
                          x.components.Select(c => new GroupComponentDto(
                                c.door.guid,
                                c.timezone.guid
                          )).ToList(),
                          x.is_active,
                          x.is_default
                    );

      public static readonly Expression<Func<Persistences.Entities.Group, CommandDetailDto>> ToCommand =
      x => new CommandDetailDto(
          -1,
          -1,
          "",
          "",
          -1,
          -1,
          Vendor.unknown,
          x.location_id,
          x.id,
          x.external_id
      );
}