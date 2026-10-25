using SharedKernel.Messaging;

namespace Core.Contract.Queries.Group;

public sealed record GroupExternalIdByGuidQuery(Guid guid) : IQuery<short>;