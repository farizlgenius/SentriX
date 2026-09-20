using SharedKernel.Messaging;

namespace Core.Contract.Queries.Device;

public sealed record DeviceGuidByIdQuery(int id) : IQuery<Guid>;