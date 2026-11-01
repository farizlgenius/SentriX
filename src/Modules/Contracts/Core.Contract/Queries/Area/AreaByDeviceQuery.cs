using Core.Contract.DTOs.Area;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Area;

public sealed record AreaByDeviceQuery(Guid guid) : IQuery<IEnumerable<AreaDto>>;