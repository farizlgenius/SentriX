using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record IsAnyMacQuery(string mac) : IQuery<bool>;