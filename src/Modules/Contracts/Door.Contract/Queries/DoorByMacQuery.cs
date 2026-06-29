using Door.Contract.DTOs;
using SharedKernel.Messaging;

namespace Door.Contract.Queries;

public sealed record DoorByMacQuery(string Mac) : IQuery<IEnumerable<DoorDto>>;