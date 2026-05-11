using System;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Queries;

public sealed class IsAnyWithMacQueryHandler : IQueryHandler<IsAnyWithMacQuery, bool>
{
      public Task<bool> HandleAsync(IsAnyWithMacQuery query, CancellationToken ct)
      {
            throw new NotImplementedException();
      }
}
