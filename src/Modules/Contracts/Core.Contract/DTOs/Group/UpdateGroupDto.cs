using Core.Contract.Dtos.Group;

namespace Core.Contract.DTOs.Group;

public sealed record UpdateGroupDto(
    Guid Guid,
    string Name,
    List<GroupComponentDto> Components,
    Guid LocationGuid
);