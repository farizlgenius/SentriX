using Core.Application.Interfaces;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputGuidByExternalIdAndMacAndVendorQueryHandler(IInputRepository repo) : IQueryHandler<InputGuidByExternalIdAndMacAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(InputGuidByExternalIdAndMacAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GuidByExternalIdAndMacAndVendorAsync(query.externalId,query.mac,query.vendor,ct);
      }
}