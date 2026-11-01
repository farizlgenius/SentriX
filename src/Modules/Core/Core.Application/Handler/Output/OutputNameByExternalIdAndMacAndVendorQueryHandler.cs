using Core.Application.Interfaces;
using Core.Contract.Queries.Output;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Output;

public sealed class OutputNameByExternalIdAndMacAndVendorQueryHandler(IOutputRepository repo) : IQueryHandler<OutputNameByExternalIdAndMacAndVendorQuery, string>
{
      public async Task<string> HandleAsync(OutputNameByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
           return await repo.GetNameByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}