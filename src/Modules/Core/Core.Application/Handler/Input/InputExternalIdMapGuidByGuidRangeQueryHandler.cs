using Core.Application.Interfaces;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputExternalIdMapGuidByGuidRangeQueryHandler(IInputRepository repo) : IQueryHandler<InputExternalIdMapGuidByGuidRangeQuery, Dictionary<Guid, short>>
{
      public async Task<Dictionary<Guid, short>> HandleAsync(InputExternalIdMapGuidByGuidRangeQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdMapGuidByGuidRangeAsync(query.guids,ct);
      }
}