using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record CompanyIdByGuidQuery(Guid guid) : IQuery<int>;