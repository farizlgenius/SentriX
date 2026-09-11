using Core.Application.Interfaces;
using Core.Contract.DTOs.Group;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class GroupService(IGroupRepository repo,
IMessageBus bus) : IGroup
{
    public async Task<Guid> CreateAsync(CreateGroupDto dto, CancellationToken ct = default)
    {
        var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid), ct);

        var doorIds = await bus.QueryAsync(new DoorIdsMapGuidsByGuidsQuery(dto.Components.Select(x => x.Doors)), ct);

        var timeZoneIds = await bus.QueryAsync(new TimeZoneIdsMapGuidsByGuidsQuery(dto.Components.Select(x => x.TimeZone)), ct);

        if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId, ct))
            throw new DuplicateException(nameof(dto.Name), dto.Name);


        var d = new Domain.Entities.Group(
            dto.Name,
            dto.Components.Select(
                x => new Domain.Entities.GroupComponent(
                    doorIds[x.Doors],
                    timeZoneIds[x.TimeZone]
                )
            ).ToList(),
            locationId
        );

        await repo.AddAsync(d, ct);

        return d.Guid;
    }

    public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
    {
        if (!await repo.IsAnyGuidAsync(guid, ct))
            throw new NotFoundException(EntityType.Group, guid.ToString());

        // Check related entities before deleting the group
        if (await repo.IsAnyRelatedEntitiesAsync(guid, ct))
            throw new FoundRelateException();

        await repo.DeleteAsync(guid, ct);

        return true;

    }

    public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
    {
        // Check if guids is empty 
        if (guids.Count() == 0)
            throw new NotFoundException(EntityType.Door);

        foreach (var guid in guids)
        {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                throw new NotFoundException(EntityType.Door, guid.ToString());

            // Check relate object here
            if (await repo.IsAnyRelatedEntitiesAsync(guid))
                throw new FoundRelateException();
        }

        await repo.DeleteRangeAsync(guids);

        return guids;
    }

    public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
    {
        // Check is any location with guid
        if (!await repo.IsAnyGuidAsync(guid, ct))
            throw new NotFoundException(EntityType.Door, guid.ToString());

        return await repo.DisableAsync(guid, ct);
    }

    public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
    {
        // Check is any location with guid
        if (!await repo.IsAnyGuidAsync(guid, ct))
            throw new NotFoundException(EntityType.Door, guid.ToString());

        return await repo.EnableAsync(guid, ct);
    }

    public async Task<GroupDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
    {
        return await repo.GetAsync(guid, ct);
    }

    public async Task<IEnumerable<GroupDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
    {
        var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(guid));
        return await repo.GetByLocationAsync(locationId, ct);
    }

    public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
    {
        return await repo.GetPaginationAsync(param, ct);
    }

    public async Task<Guid> UpdateAsync(UpdateGroupDto dto, CancellationToken ct = default)
    {
        // Check any location with guid
        if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
            throw new NotFoundException(EntityType.Group, dto.Guid.ToString());

        var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid));

        var doorIds = await bus.QueryAsync(new DoorIdsMapGuidsByGuidsQuery(dto.Components.Select(x => x.Doors)), ct);

        var timeZoneIds = await bus.QueryAsync(new TimeZoneIdsMapGuidsByGuidsQuery(dto.Components.Select(x => x.TimeZone)), ct);

        var d = new Domain.Entities.Group(
            dto.Name,
            dto.Components.Select(
                x => new Domain.Entities.GroupComponent(
                    doorIds[x.Doors],
                    timeZoneIds[x.TimeZone]
                )
            ).ToList(),
            locationId
        );

        await repo.UpdateAsync(d);

        return d.Guid;


    }
}