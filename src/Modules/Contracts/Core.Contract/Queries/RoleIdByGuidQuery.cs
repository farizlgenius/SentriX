using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record RoleIdByGuidQuery(Guid guid) : IQuery<int>;