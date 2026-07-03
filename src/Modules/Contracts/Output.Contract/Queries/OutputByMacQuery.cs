using Output.Contract.DTOs;
using SharedKernel.Messaging;

namespace Output.Contract.Queries;

public sealed record OutputByMacQuery(string Mac) : IQuery<IEnumerable<OutputDto>>;