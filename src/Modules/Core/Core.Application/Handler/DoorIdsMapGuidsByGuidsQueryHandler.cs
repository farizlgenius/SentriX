using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class DoorIdsMapGuidsByGuidsQueryHandler(IDoorRepository repo) : IQueryHandler<DoorIdsMapGuidsByGuidsQuery, Dictionary<Guid, int>>
{

    public async Task<Dictionary<Guid, int>> HandleAsync(DoorIdsMapGuidsByGuidsQuery query, CancellationToken ct = default)
    {
        foreach (var guid in query.Guids)
        {
            if (!await repo.IsAnyGuidAsync(guid, ct))
                throw new NotFoundException(EntityType.Door, guid.ToString());
        }


        return await repo.GetDoorIdsMapGuidsAsync(query.Guids, ct);
    }
}