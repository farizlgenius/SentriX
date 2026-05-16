using System;
using Adapter.Abstraction.Query;
using Adapter.Aero.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Aero.Handler;

public sealed class MaxModuleByMacQueryHandler(IScpRepository repo) : IQueryHandler<MaxModuleByMacQuery, int>
{
      public async Task<int> HandleAsync(MaxModuleByMacQuery query, CancellationToken ct)
      {
            return await repo.GetAllSioAsync();
      }
}
