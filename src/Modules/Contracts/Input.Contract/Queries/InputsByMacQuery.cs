using Input.Contract.DTOs;
using SharedKernel.Messaging;

namespace Input.Contract.Queries;

public sealed record InputByMacQuery(string Mac) : IQuery<IEnumerable<InputDto>>;