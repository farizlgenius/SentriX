using Core.Application.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.Time;
using SharedKernel.Constants;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Handler.Times;

public sealed class TimeZoneIdsMapGuidsByGuidsQueryHandler(ITimeRepository repo) : IQueryHandler<TimeZoneIdsMapGuidsByGuidsQuery, Dictionary<Guid, int>>
{
    public async Task<Dictionary<Guid, int>> HandleAsync(TimeZoneIdsMapGuidsByGuidsQuery query, CancellationToken ct = default)
    {
        foreach (var guid in query.Guids)
        {
            if (!await repo.IsAnyGuidAsync(guid))
                throw new NotFoundException(EntityType.TimeZone.ToString(), guid.ToString());
        }

        return await repo.GetTimeZoneIdsMapGuidsByGuidsAsync(query.Guids, ct);
    }
}

