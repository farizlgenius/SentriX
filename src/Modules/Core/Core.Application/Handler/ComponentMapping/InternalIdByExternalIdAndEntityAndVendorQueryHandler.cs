using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class InternalIdByExternalIdAndEntityAndVendorQueryHandler(IComponentMappingRepository repo) : IQueryHandler<InternalIdByExternalIdAndEntityAndVendorQuery, int>
{
      public async Task<int> HandleAsync(InternalIdByExternalIdAndEntityAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetInternalIdByExternalIdAndEntityAndVendorAsync(query.externalId,query.entity,query.vendor,ct);
      }
}