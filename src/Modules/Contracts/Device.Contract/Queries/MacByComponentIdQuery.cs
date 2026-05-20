using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record MacByComponentIdQuery(int ComponentId) : IQuery<string>;