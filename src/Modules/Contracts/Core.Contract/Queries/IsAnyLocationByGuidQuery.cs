using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record IsAnyLocationByGuidQuery(Guid LocationGuid) : IQuery<bool>;