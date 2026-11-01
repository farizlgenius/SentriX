using Core.Contract.DTOs.Output;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record OutputByDeviceQuery(Guid guid) : IQuery<IEnumerable<OutputDto>>;