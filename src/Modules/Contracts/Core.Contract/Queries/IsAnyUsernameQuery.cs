using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record IsAnyUsernameQuery(string username) : IQuery<bool>;