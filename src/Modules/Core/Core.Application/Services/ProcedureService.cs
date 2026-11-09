using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Procedure;
using Core.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ProcedureService(
      IActionRepository action,
      IProcedureRepository repo,
      IDeviceRepository device,
      IAdapterFactory adapter
      ) : BaseService<ProcedureDto, Domain.Entities.Procedure>(repo), IProcedure
{
      public async Task<Guid> CreateAsync(CreateProcedureDto dto, CancellationToken ct = default)
      {
            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid, ct);
            var actions = await action.GetActionDetailByGuidAsync(dto.Actions, ct);

            // Check name in same location 

            if (await Repo.IsAnyByNameAndLocationIdAsync(dto.Name, dev.LocationId ?? 1, ct))
                  throw new DuplicateException(EntityType.PROCEDURE.ToString(), nameof(dto.Name));

            var externalId = await Repo.GetFreeExternalIdAsync(100, [], ct);

            var d = new Domain.Entities.Procedure(
                  dto.Name,
                  dev.DeviceId,
                  dev.LocationId ?? 1,
                  actions.Select(x => x.id),
                  externalId
            );

            // Send command to controller 
            await adapter.GetAdapter(dev.Vendor).Procedure.AddProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  externalId,
                  actions.ToDictionary(x => x.type, x => x.metadata),
                  ct
            );

            await repo.AddAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid, ct);

            if (await Repo.IsAnyRelatedEntitiesAsync(guid, ct))
                  throw new DuplicateException(EntityType.PROCEDURE.ToString(), EntityType.TRIGGER.ToString());

            // Command 

            await adapter.GetAdapter(dev.Vendor).Procedure.DeleteProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            await Repo.DeleteAsync(guid, ct);

            return true;

      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            // Check if guids is empty 
            if (!guids.Any())
                  throw new NotFoundException(EntityType.PROCEDURE.ToString());

            foreach (var guid in guids)
            {
                  // Check is any location with guid
                  if (!await Repo.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.COMPANY.ToString(), guid.ToString());

                  // Check relate object here

                  if (await Repo.IsAnyRelatedEntitiesAsync(guid, ct))
                        throw new FoundRelateException(EntityType.PROCEDURE.ToString(), guid.ToString(), EntityType.TRIGGER.ToString());
            }

            await Repo.DeleteRangeAsync(guids, ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await Repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.PROCEDURE.ToString(), guid.ToString());

            return await Repo.DisableAsync(guid, ct);
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await Repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.PROCEDURE.ToString(), guid.ToString());

            return await Repo.EnableAsync(guid, ct);
      }

      public async Task<Guid> UpdateAsync(UpdateProcedureDto dto, CancellationToken ct = default)
      {
            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid, ct);
            var actions = await action.GetActionDetailByGuidAsync(dto.Actions, ct);

            // Check name in same location 

            if (await Repo.IsAnyByNameAndLocationIdExceptCurrentAsync(dto.Name,dto.Guid,dev.LocationId ?? 1,ct))
                  throw new DuplicateException(EntityType.PROCEDURE.ToString(), nameof(dto.Name));

            var externalId = await Repo.GetFreeExternalIdAsync(100, [], ct);
            

            var d = new Domain.Entities.Procedure(
                  dto.Name,
                  dev.DeviceId,
                  dev.LocationId ?? 1,
                  actions.Select(x => x.id),
                  externalId
            );

            // Send command to controller 
            await adapter.GetAdapter(dev.Vendor).Procedure.AddProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  externalId,
                  actions.ToDictionary(x => x.type, x => x.metadata),
                  ct
            );

            await repo.AddAsync(d, ct);

            return d.Guid;
      }
}