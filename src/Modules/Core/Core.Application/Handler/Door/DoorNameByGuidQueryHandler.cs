using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Door;

public sealed class DoorNameByGuidQueryHandler(IDoorRepository repo) : IQueryHandler<DoorNameByGuidQuery, string>
{
      public async Task<string> HandleAsync(DoorNameByGuidQuery query, CancellationToken ct)
      {
            return await repo.GetNameByGuidAsync(query.guid,ct);
      }
}