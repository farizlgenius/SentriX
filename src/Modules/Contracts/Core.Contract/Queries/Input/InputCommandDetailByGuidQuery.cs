using Core.Contract.DTOs;
using SharedKernel.Messaging;

namespace Core.Contract.Queries.Input;

public sealed record InputCommandDetailByGuidQuery(Guid guid) : IQuery<CommandDetailDto>;