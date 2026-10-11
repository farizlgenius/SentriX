using SharedKernel.Enums;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record DoorNameByGuidQuery(Guid guid) : IQuery<string>;