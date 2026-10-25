using SharedKernel.Messaging;

namespace Core.Contract.Queries.DeviceModule;

public sealed record DeviceModuleExternalIdByGuidQuery(Guid guid) : IQuery<short>;