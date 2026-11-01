using SharedKernel.Messaging;

namespace Core.Contract.Queries.Device;

public sealed record IsAnyDeviceExeternalIdQuery(short externalId) : IQuery<bool>;