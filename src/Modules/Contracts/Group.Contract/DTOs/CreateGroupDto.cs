using SharedKernel.Domain;

namespace Group.Contract.DTOs;

public sealed record CreateGroupDto(string Name,string Metadata,int LocationId, bool IsActive) : BaseDto(0, LocationId, string.Empty, IsActive);