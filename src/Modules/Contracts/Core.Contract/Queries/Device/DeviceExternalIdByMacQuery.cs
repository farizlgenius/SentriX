using SharedKernel.Messaging;

namespace Core.Contract.Queries.Device;

public sealed record DeviceExternalIdByMacQuery(string mac) : IQuery<short>;