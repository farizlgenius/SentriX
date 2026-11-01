using Core.Contract.DTOs;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Area;

public sealed record AreaCommandDetailByGuidQuery(Guid guid) : IQuery<CommandDetailDto>;