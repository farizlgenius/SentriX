using System.Linq.Expressions;
using Core.Contract.Dtos.Group;
using Core.Contract.DTOs;
using SharedKernel.Enums;

namespace Core.Infrastructure.Projections;

public static class GroupProjections
{

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

      public static readonly Expression<Func<Persistences.Entities.Group, Core.Contract.DTOs.Group.GroupDto>> ToDto =
          x => new Core.Contract.DTOs.Group.GroupDto(
            x.guid,
            x.name,
            new List<GroupComponentDto>(),
            x.location == null ? Guid.Empty : x.location.guid,
            x.is_active,
            x.is_default
            );
}