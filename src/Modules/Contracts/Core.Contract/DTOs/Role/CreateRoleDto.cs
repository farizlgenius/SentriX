namespace Core.Contract.DTOs.Role;

public sealed record CreateRoleDto(
      string Name,
      List<ModuleDto> Modules,
      List<Guid> LocationGuids
);