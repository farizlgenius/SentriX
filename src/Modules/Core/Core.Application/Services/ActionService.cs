using Core.Application.Interfaces;
using Core.Contract.DTOs.Action;
using Core.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ActionService(
      IActionRepository repo,
      IDeviceRepository device,
      ILocationRepository loc
      ) : BaseService<ActionDto,Domain.Entities.Actions>(
            repo
      ),IAction
{
      public async Task<Guid> CreateAsync(CreateActionDto dto, CancellationToken ct = default)
      {
            var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid,ct);
            var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid,ct);

            // Check name in same location 

            if(await Repo.IsAnyByNameAndLocationGuidAsync(dto.Name,dto.LocationGuid,ct))
                  throw new DuplicateException(EntityType.ACTION.ToString(),nameof(dto.Name));

            var d = new Domain.Entities.Actions(
                  dto.Name,
                  dto.Type,
                  dto.Metadata,
                  deviceId,
                  locationId
            );

            await repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check any relate first 

            if(await repo.IsAnyRelatedEntitiesAsync(guid,ct))
                  throw new FoundRelateException();

            await repo.DeleteAsync(guid,ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            // Check if guids is empty 
            if (!guids.Any())
                  throw new NotFoundException(EntityType.ACTION.ToString());

            foreach (var guid in guids)
            {
                  // Check is any location with guid
                  if (!await repo.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

                  // Check relate object here
                  if (await repo.IsAnyRelatedEntitiesAsync(guid, ct))
                        throw new FoundRelateException(EntityType.ACTION.ToString(), guid.ToString(), EntityType.PROCEDURE.ToString());
            }

            await repo.DeleteRangeAsync(guids, ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

            return await repo.DisableAsync(guid, ct);
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

            return await repo.EnableAsync(guid, ct);
      }


      public async Task<Guid> UpdateAsync(UpdateActionDto dto, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await repo.IsAnyGuidAsync(dto.Guid,ct))
                  throw new NotFoundException(EntityType.ACTION.ToString(), dto.Guid.ToString());

            if(await Repo.IsAnyByNameAndLocationGuidExceptCurrentAsync(dto.Name,dto.Guid,dto.LocationGuid,ct))
                  throw new DuplicateException(EntityType.ACTION.ToString(),nameof(dto.Name));

            var deviceId = await device.GetIdByGuidAsync(dto.DeviceGuid, ct);
            var locationId = await loc.GetIdByGuidAsync(dto.LocationGuid, ct);

            var d = new Domain.Entities.Actions(
                  dto.Name,
                  dto.Type,
                  dto.Metadata,
                  deviceId,
                  locationId
            );

            await repo.UpdateAsync(d, ct);

            return d.Guid;
      }
}