using Core.Application.Interfaces;
using Core.Contract.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class GuidByExternalIdAndEntityAndVendorQueryHandler(IComponentMappingRepository repo) : IQueryHandler<GuidByExternalIdAndEntityAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(GuidByExternalIdAndEntityAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalIdAndEntityAndVendorAsync(query.externalId,query.entity,query.vendor,ct);
      }
}