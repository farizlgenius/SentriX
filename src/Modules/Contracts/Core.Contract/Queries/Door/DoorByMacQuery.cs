using Core.Contract.DTOs.Door;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Door;

public sealed record DoorByMacQuery(string mac) : IQuery<IEnumerable<DoorDto>>;