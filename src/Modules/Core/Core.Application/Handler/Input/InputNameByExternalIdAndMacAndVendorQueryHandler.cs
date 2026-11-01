using Core.Application.Interfaces;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputNameByExternalIdAndMacAndVendorQueryHandler(IInputRepository repo) : IQueryHandler<InputNameByExternalIdAndMacAndVendorQuery, string>
{
      public async Task<string> HandleAsync(InputNameByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GetNameByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}