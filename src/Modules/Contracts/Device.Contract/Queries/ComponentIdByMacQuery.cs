using SharedKernel.Messaging;

namespace Device.Contract.Queries;

public sealed record ComponentIdByMacQuery(string Mac) : IQuery<int>;