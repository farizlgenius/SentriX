using Core.Application.Interfaces;
using Core.Contract.Queries.DeviceModule;
using Core.Contract.Queries.Door;
using SharedKernel.Messaging;

namespace Core.Application.Handler.DeviceModule;

public sealed class DoorGuidByExternalIdAndMacAndVendorQueryHandler(IDoorRepository repo) : IQueryHandler<DoorGuidByExternalIdAndMacAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(DoorGuidByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetGuidByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}