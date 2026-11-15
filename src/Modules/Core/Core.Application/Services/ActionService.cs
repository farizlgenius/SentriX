using Core.Application.Interfaces;
using Core.Contract.DTOs.Action;
using Core.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ActionService(
      IActionRepository repo,
      IInputRepository input,
      IOutputRepository output,
      IDoorRepository door,
      IAreaRepository area,
      IMonitorGroupRepository monitorGroup,
      ITimeRepository time,
      IProcedureRepository procedure,
      ITriggerVariableRepository triggerVariable,
      IDeviceRepository device
      ) : BaseService<ActionDto,Domain.Entities.Actions>(
            repo
      ),IAction
{
      public async Task<Guid> CreateAsync(CreateActionDto dto, CancellationToken ct = default)
      {
            short? externalId = null;
            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid,ct);

            // Check name in same location 

            if(await Repo.IsAnyByNameAndLocationIdAsync(dto.Name,dev.LocationId ?? 1,ct))
                  throw new DuplicateException(EntityType.ACTION.ToString(),nameof(dto.Name));

            switch (dto.Type)
            {
                  case ActionType.MonitorPointMask:
                  externalId = dto.SrcGuid == null ? null : await input.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ControlPointCommand:
                  externalId = dto.SrcGuid == null ? null : await output.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.DoorMode:
                  case ActionType.ForceOpenMask:
                  case ActionType.HeldOpenMask:
                  case ActionType.MomentaryUnlock:
                  case ActionType.ReaderLEDModeControl:
                  case ActionType.AntipassbackControlCommand:
                  case ActionType.AbortWaitForDoorOpenState:
                  case ActionType.TempReaderLEDControl:
                  case ActionType.TextLCDTerminal:
                  case ActionType.HostSimulateCardRead:
                  externalId = dto.SrcGuid == null ? null : await door.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ProcedureControlCommand:
                  externalId = dto.SrcGuid == null ? null : await procedure.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.TriggerVariableControlCommand:
                   externalId = dto.SrcGuid == null ? null : await triggerVariable.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.TimeZoneControl:
                  externalId = dto.SrcGuid == null ? null : await time.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.MonitorPointGroupArmDisarmCommand:
                  externalId = dto.SrcGuid == null ? null : await monitorGroup.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ModifyAreaConfiguration:
                  externalId = dto.SrcGuid == null ? null : await area.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.SetUseLimit:
                  case ActionType.SetOperationMode:
                  case ActionType.HostSimulatedKeyPress:
                  case ActionType.FilterOutTriggerActivationSummary:
                  case ActionType.TriggerActivationSummary:
                  case ActionType.DelayMillisecIncrement:
                  case ActionType.DelayMinIncrement:
                  case ActionType.DeleteAll:
                  default:
                  break;
            }

            var d = new Domain.Entities.Actions(
                  dto.Name,
                  dto.Type,
                  dev.DeviceId,
                  dev.LocationId ?? 1,
                  externalId,
                  dto.Metadata
            );

            await Repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check any relate first 

            if(await Repo.IsAnyRelatedEntitiesAsync(guid,ct))
                  throw new FoundRelateException();

            await Repo.DeleteAsync(guid,ct);

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
                  if (!await Repo.IsAnyGuidAsync(guid, ct))
                        throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

                  // Check relate object here
                  if (await Repo.IsAnyRelatedEntitiesAsync(guid, ct))
                        throw new FoundRelateException(EntityType.ACTION.ToString(), guid.ToString(), EntityType.PROCEDURE.ToString());
            }

            await Repo.DeleteRangeAsync(guids, ct);

            return guids;
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await Repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

            return await Repo.DisableAsync(guid, ct);
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            if (!await Repo.IsAnyGuidAsync(guid, ct))
                  throw new NotFoundException(EntityType.ACTION.ToString(), guid.ToString());

            return await Repo.EnableAsync(guid, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetActionOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetActionOptionByDeviceAsync(guid,ct);
      }

      public async Task<IEnumerable<OptionDto>> GetActionOptionByTypeAndDeviceAsync(Guid guid, ActionType type, CancellationToken ct = default)
      {
            return await repo.GetActionOptionByTypeAndDeviceAsync(guid,type,ct);
      }

      public async Task<Guid> UpdateAsync(UpdateActionDto dto, CancellationToken ct = default)
      {
            short? externalId = null;
            var dev = await Repo.GetCommandDetailAsync(dto.Guid,ct);

            if(await Repo.IsAnyByNameAndLocationIdExceptCurrentAsync(dto.Name,dto.Guid,dev.LocationId ?? 1,ct))
                  throw new DuplicateException(EntityType.ACTION.ToString(),nameof(dto.Name));

            switch (dto.Type)
            {
                  case ActionType.MonitorPointMask:
                  externalId = dto.SrcGuid == null ? null : await input.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ControlPointCommand:
                  externalId = dto.SrcGuid == null ? null : await output.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.DoorMode:
                  case ActionType.ForceOpenMask:
                  case ActionType.HeldOpenMask:
                  case ActionType.MomentaryUnlock:
                  case ActionType.ReaderLEDModeControl:
                  case ActionType.AntipassbackControlCommand:
                  case ActionType.AbortWaitForDoorOpenState:
                  case ActionType.TempReaderLEDControl:
                  case ActionType.TextLCDTerminal:
                  case ActionType.HostSimulateCardRead:
                  externalId = dto.SrcGuid == null ? null : await door.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ProcedureControlCommand:
                  externalId = dto.SrcGuid == null ? null : await procedure.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.TriggerVariableControlCommand:
                   externalId = dto.SrcGuid == null ? null : await triggerVariable.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.TimeZoneControl:
                  externalId = dto.SrcGuid == null ? null : await time.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.MonitorPointGroupArmDisarmCommand:
                  externalId = dto.SrcGuid == null ? null : await monitorGroup.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.ModifyAreaConfiguration:
                  externalId = dto.SrcGuid == null ? null : await area.GetExternalIdByGuidAsync(dto.SrcGuid ?? Guid.Empty,ct);
                  break;
                  case ActionType.SetUseLimit:
                  case ActionType.SetOperationMode:
                  case ActionType.HostSimulatedKeyPress:
                  case ActionType.FilterOutTriggerActivationSummary:
                  case ActionType.TriggerActivationSummary:
                  case ActionType.DelayMillisecIncrement:
                  case ActionType.DelayMinIncrement:
                  case ActionType.DeleteAll:
                  default:
                  break;
            }

            var d = new Domain.Entities.Actions(
                  dto.Guid,
                  dto.Name,
                  dto.Type,
                  dev.DeviceId,
                  dev.LocationId ?? 1,
                  externalId,
                  dto.Metadata
            );

            await Repo.UpdateAsync(d, ct);

            return d.Guid;
      }
}