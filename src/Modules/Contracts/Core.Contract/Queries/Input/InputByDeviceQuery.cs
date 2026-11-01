using Core.Contract.DTOs.Input;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputByDeviceQuery(Guid guid) : IQuery<IEnumerable<InputDto>>;