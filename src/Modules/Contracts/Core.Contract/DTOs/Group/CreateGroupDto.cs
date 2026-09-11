using Core.Contract.Dtos.Group;

namespace Core.Contract.DTOs.Group;

public sealed record CreateGroupDto(
    string Name,
    List<GroupComponentDto> Components,
    Guid LocationGuid
);