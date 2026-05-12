using SharedKernel.Messaging;

namespace Location.Contract.Queries;

public sealed record IsAnyLocationByIdQuery(int LocationId) : IQuery<bool>;
