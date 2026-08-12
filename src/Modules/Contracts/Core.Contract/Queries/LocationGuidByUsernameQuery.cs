using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record LocationGuidByUsernameQuery(string username) : IQuery<IEnumerable<Guid>>;