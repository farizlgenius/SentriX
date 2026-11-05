using Core.Application.Interfaces;
using Core.Contract.Queries.Area;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Area;

public sealed class AreaNameByExternalIdAndMacAndVendorQueryHandler(IAreaRepository repo) : IQueryHandler<AreaNameByExternalIdAndMacAndVendorQuery, string>
{
      public async Task<string> HandleAsync(AreaNameByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
           return await repo.GetNameByExternalIdAndMacAndVendorAsync(query.ExternalId,query.Mac,query.Vendor,ct);
      }
}