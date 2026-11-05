using Core.Application.Interfaces;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputGuidByExternalIdAndMacAndVendorQueryHandler(IOutputRepository repo) : IQueryHandler<OutputGuidByExternalIdAndMacAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(OutputGuidByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.OutputGuidByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}