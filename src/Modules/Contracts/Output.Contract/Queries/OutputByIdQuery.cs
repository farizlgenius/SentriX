using Output.Contract.DTOs;
using SharedKernel.Messaging;

namespace Output.Contract.Queries;

public sealed record OutputByIdQuery(int Id) : IQuery<OutputDto>;