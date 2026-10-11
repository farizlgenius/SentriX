using Core.Application.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class GuidByExternalIdAndMacAndEntityAndVendorQueryHandler(IComponentMappingRepository repo) : IQueryHandler<GuidByExternalIdAndMacAndEntityAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(GuidByExternalIdAndMacAndEntityAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GuidByExternalIdAndMacAndEntityAndVendorQuery(query.externalId,query.mac,query.entity,query.vendor,ct);
      }
}
