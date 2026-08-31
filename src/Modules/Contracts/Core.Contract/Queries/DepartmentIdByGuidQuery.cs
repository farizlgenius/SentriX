using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record DepartmentIdByGuidQuery(Guid guid) : IQuery<int>;