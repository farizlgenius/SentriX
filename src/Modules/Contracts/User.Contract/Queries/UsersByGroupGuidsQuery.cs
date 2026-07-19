using SharedKernel.Messaging;
using User.Contract.DTOs;

namespace User.Contract.Queries;

public sealed record UsersByGroupGuidsQuery(IEnumerable<Guid> guids) : IQuery<IEnumerable<UserDto>>;