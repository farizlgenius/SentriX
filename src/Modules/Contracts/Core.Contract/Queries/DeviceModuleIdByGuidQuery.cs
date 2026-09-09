using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record DeviceModuleIdByGuidQuery(Guid guid) : IQuery<int>;