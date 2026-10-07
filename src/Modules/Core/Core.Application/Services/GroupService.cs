using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Group;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using Core.Contract.Queries.Time;
using SharedKernel.Constants;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class GroupService(
    IGroupRepository repo,
    IDoorRepository door,
    ILocationRepository loc,
    ITimeRepository time,
    IAdapterFactory adapter,
    IComponentMappingRepository com,
    IMessageBus bus) : IGroup
{
    public async Task<Guid> CreateAsync(CreateGroupDto dto, CancellationToken ct = default)
    {
        //var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(dto.LocationGuid), ct);
        var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

        //var doorIds = await bus.QueryAsync(new DoorIdsMapGuidsByGuidsQuery(dto.Components.Select(x => x.Doors)), ct);
        //var doorIds = await door.GetDoorIdsMapGuidsAsync(dto.Components.Select(x => x.Doors),ct);
        var doorDetails = await door.GetDetailsByGuidAsync(dto.Components.Select(x => x.Doors),ct);

        //var timeZoneIds = await bus.QueryAsync(new TimeZoneIdsMapGuidsByGuidsQuery(dto.Components.Select(x => x.TimeZone)), ct);
        var timeZoneIds = await time.GetTimeZoneIdsMapGuidsByGuidsAsync(dto.Components.Select(x => x.TimeZone),ct);

        if (await repo.IsAnyByNameAndLocationIdAsync(dto.Name, locationId, ct))
            throw new DuplicateException(nameof(dto.Name), dto.Name);


        var d = new Domain.Entities.Group(
            dto.Name,
            dto.Components.Select(
                x => new Domain.Entities.GroupComponent(
                    doorDetails.FirstOrDefault(d => d.guid == x.Doors).id,
                    timeZoneIds[x.TimeZone]
                )
            ).ToList(),
            locationId
        );

        // Send command here.
        var doorExternalIds = await com.GetExternalIdMapGuidByGuidsAndEntityAsync(dto.Components.Select(x => x.Doors),EntityType.Door,ct);
        var timeExternalIds = await com.GetExternalIdMapGuidByGuidsAndEntityAsync(dto.Components.Select(x => x.TimeZone),EntityType.TimeZone,ct);

        var uniqueDevice = doorDetails
            .GroupBy(x => x.mac)
            .Select(x => new
            {
                Mac=x.Key,
                Vendor=x.First().vendor,
                Ip = x.First().ip,
                Guids=x.Select(x => x.guid).ToList(),
            } )
            .ToList();


        foreach(var dev in uniqueDevice) // Each Mac
        {
            var groupExternalId = await com.GetFreeIdByMacAndEntityAndVendorAsync(dev.Mac,EntityType.Group,dev.Vendor,100,[],ct);
            var deviceId = await com.GetExternalIdByMacAndEntityAsync(dev.Mac,EntityType.Device,ct);

            await adapter.GetAdapter(dev.Vendor).Group.AddAccessGroupAsync(
                dev.Mac,
                dev.Ip,
                (short)deviceId,
                (short)groupExternalId,
                dto.Components.Where(x => dev.Guids.Contains(x.Doors)).Select(x => ((short)doorExternalIds[x.Doors], (short)timeExternalIds[x.TimeZone])).ToList()
            );
        }



        await repo.AddAsync(d, ct);

        return d.Guid;
    }

    public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
    {
        if (!await repo.IsAnyGuidAsync(guid, ct))
            throw new NotFoundException(EntityType.Group.ToString(), guid.ToString());

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
            throw new NotFoundException(EntityType.Door.ToString());

        foreach (var guid in guids)
        {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

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
            throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

        return await repo.DisableAsync(guid, ct);
    }

    public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
    {
        // Check is any location with guid
        if (!await repo.IsAnyGuidAsync(guid, ct))
            throw new NotFoundException(EntityType.Door.ToString(), guid.ToString());

        return await repo.EnableAsync(guid, ct);
    }

    public async Task<GroupDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
    {
        return await repo.GetAsync(guid, ct);
    }

    public async Task<IEnumerable<GroupDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
    {
        return await repo.GetByLocationAsync(guid, ct);
    }

    public async Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
    {
        return await repo.GetPaginationAsync(param, ct);
    }

    public async Task<Guid> UpdateAsync(UpdateGroupDto dto, CancellationToken ct = default)
    {
        // Check any location with guid
        if (!await repo.IsAnyGuidAsync(dto.Guid, ct))
            throw new NotFoundException(EntityType.Group.ToString(), dto.Guid.ToString());

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

      public Task UploadAsync(CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }
}