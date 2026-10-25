using Core.Application.Interfaces;
using Core.Contract.Queries.Door;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Door;

public sealed class DoorNameByExternalIdAndMacAndVendorQueryHandler(IDoorRepository repo) : IQueryHandler<DoorNameByExternalIdAndMacAndVendorQuery, string>
{
      public async Task<string> HandleAsync(DoorNameByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetNameByExternalIdAndMacAndVendorQuery(query.externalId,query.mac,query.Vendor,ct);
      }
}