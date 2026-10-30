using Core.Application.Interfaces;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputGuidByExternalIdAndDeviceExternalIdAndVendorQueryHandler(IOutputRepository repo) : IQueryHandler<OutputGuidByExternalIdAndDeviceExternalIdAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(OutputGuidByExternalIdAndDeviceExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.OutputGuidByExternalIdAndDeviceExternalIdAndVendorAsync(query.externalId,query.deivceExternalId,query.vendor,ct);
      }
}