using Core.Contract.Dtos.Group;

namespace Core.Contract.DTOs.Group;

public sealed record GroupDto(
    Guid Guid,
    string Name,
    List<GroupComponentDto> Components,
    bool IsActive,
    bool IsDefault
);