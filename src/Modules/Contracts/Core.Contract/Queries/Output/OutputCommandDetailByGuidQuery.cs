using Core.Contract.DTOs;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Output;

public sealed record OutputCommandDetailByGuidQuery(Guid guid) : IQuery<CommandDetailDto>;
