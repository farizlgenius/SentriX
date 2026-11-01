using Core.Application.Interfaces;
using Core.Contract.Queries.Input;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Input;

public sealed class InputGuidByExternalIdAndDeviceExternalIdAndVendorQueryHandler(IInputRepository repo) : IQueryHandler<InputGuidByExternalIdAndDeviceExternalIdAndVendorQuery, Guid>
{
      public async Task<Guid> HandleAsync(InputGuidByExternalIdAndDeviceExternalIdAndVendorQuery query, CancellationToken ct)
      {
            return await repo.GuidByExternalIdAndDeviceExternalIdAndVendorAsync(query.externalId,query.deivceExternalId,query.vendor,ct);
      }
}