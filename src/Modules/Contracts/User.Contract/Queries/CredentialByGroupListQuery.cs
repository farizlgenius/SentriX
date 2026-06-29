using SharedKernel.Messaging;
using User.Contract.DTOs;

namespace User.Contract.Queries;

public sealed record CredentialByGroupListQuery(List<int> Groups) : IQuery<IEnumerable<CredentialDto>>;