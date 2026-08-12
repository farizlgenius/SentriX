using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record HashedByUsernameQuery(string username) : IQuery<string>;