using Input.Contract.DTOs;
using SharedKernel.Messaging;

namespace Input.Contract.Queries;

public sealed record InputGroupByMacQuery(string Mac) : IQuery<IEnumerable<InputGroupDto>>;