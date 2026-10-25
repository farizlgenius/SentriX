using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record OutputExternalIdByGuidQuery(Guid guid) : IQuery<short>;