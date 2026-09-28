using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Constants;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class ExternalIdByMacAndEntityQueryHandler(IComponentMappingRepository repo) : IQueryHandler<ExternalIdByMacAndEntityQuery, int>
{
      public async Task<int> HandleAsync(ExternalIdByMacAndEntityQuery query, CancellationToken ct)
      {
            var res = await repo.GetExternalIdByMacAndEntityAsync(query.mac,query.entity);

            return res;
      }
}