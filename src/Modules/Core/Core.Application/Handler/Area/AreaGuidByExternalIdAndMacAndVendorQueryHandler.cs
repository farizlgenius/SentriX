using Core.Application.Interfaces;
using Core.Contract.Queries.Area;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Area;

public sealed class AreaGuidByExternalIdAndMacAndVendorQueryHandler(IAreaRepository repo) : IQueryHandler<AreaGuidByExternalIdAndMacAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(AreaGuidByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalIdAndMacAndVendorAsync(query.ExternalId,query.Mac,query.Vendor,ct);
      }
}