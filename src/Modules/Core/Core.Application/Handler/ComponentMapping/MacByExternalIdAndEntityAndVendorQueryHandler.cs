using Core.Contract.Interfaces;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Messaging;

namespace Core.Application.Handler.ComponentMapping;

public sealed class MacByExternalIdAndEntityAndVendorQueryHandler(IComponentMapping com) : IQueryHandler<MacByExternalIdAndEntityAndVendorQuery, string>
{
      public async Task<string> HandleAsync(MacByExternalIdAndEntityAndVendorQuery query, CancellationToken ct)
      {
           return await com.GetMacByExternalIdAndEntityAndVendorAsync(query.externalId,query.entity,query.vendor,ct);
      }
}