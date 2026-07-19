using Group.Contract.DTOs;
using SharedKernel.Messaging;

namespace Group.Contract.Queries;

public sealed record GroupGuidsByMacQuery(
      string Mac
      ) : IQuery<IEnumerable<(Guid guid,short componentId)>>;