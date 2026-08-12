using Core.Contract.DTOs.Role;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record PermissionByRoleGuidQuery(Guid roleGuid) : IQuery<IEnumerable<PermissionDto>>;