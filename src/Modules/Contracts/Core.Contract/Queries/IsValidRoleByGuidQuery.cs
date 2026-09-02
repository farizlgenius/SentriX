using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record IsValidRoleByGuidQuery(Guid RoleGuid) : IQuery<bool>;