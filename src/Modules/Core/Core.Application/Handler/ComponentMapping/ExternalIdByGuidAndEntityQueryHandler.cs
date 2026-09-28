using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class ExternalIdByGuidAndEntityQueryHandler(IComponentMappingRepository repo) : IQueryHandler<ExternalIdByGuidAndEntityQuery, int>
{
      public async Task<int> HandleAsync(ExternalIdByGuidAndEntityQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByGuidAndEntityAsync(query.guid,query.entity,ct);
      }
}