using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record InputExternalIdByGuidQuery(Guid guid) : IQuery<short>;