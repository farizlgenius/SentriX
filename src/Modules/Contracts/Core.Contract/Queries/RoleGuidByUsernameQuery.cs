using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record RoleGuidByUsernameQuery(string username) : IQuery<Guid>;