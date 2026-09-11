using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record DeviceModuleIdsMapGuidsByGuidsQuery(IEnumerable<Guid> guids) : IQuery<Dictionary<Guid, int>>;
