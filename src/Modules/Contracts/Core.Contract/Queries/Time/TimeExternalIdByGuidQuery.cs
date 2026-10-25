using SharedKernel.Messaging;

namespace Core.Contract.Queries.Time;

public sealed record TimeExternalIdByGuidQuery(Guid guid) : IQuery<short>;