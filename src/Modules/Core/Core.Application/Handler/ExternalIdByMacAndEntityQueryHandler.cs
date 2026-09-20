using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class ExternalIdByMacAndEntityQueryHandler(IComponentMappingRepository repo) : IQueryHandler<ExternalIdByMacAndEntityQuery, int>
{
      public async Task<int> HandleAsync(ExternalIdByMacAndEntityQuery query, CancellationToken ct)
      {
            return await repo.GetExternalIdByMacAndEntityAsync(query.mac,query.entity);
      }
}