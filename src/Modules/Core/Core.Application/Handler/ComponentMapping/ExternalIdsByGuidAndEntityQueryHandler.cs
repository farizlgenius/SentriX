using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class ExternalIdsByGuidAndEntityQueryHandler(IComponentMappingRepository repo) : IQueryHandler<ExternalIdsByGuidAndEntityQuery, IEnumerable<int>>
{
      public async Task<IEnumerable<int>> HandleAsync(ExternalIdsByGuidAndEntityQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdsByGuidAndEntityAsync(query.guid,query.entity,ct);
      }
}