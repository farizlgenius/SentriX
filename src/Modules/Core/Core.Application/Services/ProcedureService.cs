using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Procedure;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class ProcedureService(
      IActionRepository action,
      IProcedureRepository repo,
      IDeviceRepository device,
      IAdapterFactory adapter,
      IMonitorRepository monitor,
      IControlRepository control,
      IDoorRepository door,
      IAreaRepository area,
      ITriggerRepository trigger,
      ITimeRepository time,
      IUserRepository user,
      IMonitorGroupRepository monitorGroup,
      ITriggerVariableRepository triggerVar
      ) : BaseService<ProcedureDto, Domain.Entities.Procedure>(repo), IProcedure
{
      public async Task<Guid> CreateAsync(CreateProcedureDto dto, CancellationToken ct = default)
      {
            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid, ct);

            // Check name in same location 

            if (await Repo.IsAnyByNameAndLocationIdAsync(dto.Name, dev.LocationId ?? 1, ct))
                  throw new DuplicateException(EntityType.PROCEDURE.ToString(), nameof(dto.Name));

            var externalId = await Repo.GetFreeExternalIdAsync(100, [], ct);

            // Extracted 

            Dictionary<ActionType,IEnumerable<(int id,short externalId)>> extractedAction = [];

            foreach(var action in dto.Actions)
            {
                  switch (action.Type)
                  {
                        case ActionType.MonitorPointMask:
                        if(action.TargetGuids != null)
                        {
                              var datas = await monitor.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);

                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.ControlPointCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await control.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.DoorMode:
                        case ActionType.ForceOpenMask:
                        case ActionType.HeldOpenMask:
                        case ActionType.MomentaryUnlock:
                        case ActionType.ReaderLEDModeControl:
                        case ActionType.AbortWaitForDoorOpenState:
                        case ActionType.TempReaderLEDControl:
                        case ActionType.TempDoorMode:
                        case ActionType.HostSimulateCardRead:
                        case ActionType.HostSimulatedKeyPress:
                        if(action.TargetGuids != null)
                        {
                              var datas = await door.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.ProcedureControlCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await repo.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.TriggerVariableControlCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await triggerVar.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.TimeZoneControl:
                        if(action.TargetGuids != null)
                        {
                              var datas = await time.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.AntipassbackControlCommand:
                        case ActionType.SetUseLimit:
                        if(action.TargetGuids != null)
                        {
                              var datas = await user.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.MonitorPointGroupArmDisarmCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await monitorGroup.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.ModifyAreaConfiguration:
                        if(action.TargetGuids != null)
                        {
                              var datas = await area.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.TriggerActivationSummary:
                        case ActionType.FilterOutTriggerActivationSummary:
                        if(action.TargetGuids != null)
                        {
                              var datas = await trigger.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.DelayMinIncrement:
                        case ActionType.DelayMillisecIncrement:
                        case ActionType.DeleteAll:
                        default:
                        break;
                  }
            }


            var d = new Domain.Entities.Procedure(
                  dto.Name,
                  dev.DeviceId,
                  dev.LocationId ?? 1,
                  dto.Actions.Select((x,i) => new Domain.Entities.Action(
                        x.Name,
                        x.Type,
                        dev.DeviceId,
                        dev.LocationId ?? 1,
                        (short)i,
                        [.. extractedAction.Where(e => e.Key == x.Type).SelectMany(x => x.Value)],
                        x.Metadata
                  )),
                  externalId
                  );

            // Send command to controller 
            await adapter.GetAdapter(dev.Vendor).Procedure.AddProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  externalId,
                  d.Actions.SelectMany(x => x.ActionTarget.Select(a => (x.Type,x.Metadata,a.ExternalId,x.Sequence))),
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
            var dev = await repo.GetCommandDetailAsync(guid, ct);

            await adapter.GetAdapter(dev.Vendor).Procedure.DeleteProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            return await Repo.DisableAsync(guid, ct);
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            // Check is any location with guid
            var dev = await repo.GetCommandDetailAsync(guid, ct);

            var proc = await repo.GetAsync(guid,ct);

            Dictionary<Guid,(int id,short? externalId)> extractedAction = [];

            foreach(var action in proc.Actions)
            {
                  switch (action.Type)
                  {
                        case ActionType.MonitorPointMask:
                        if(action.TargetGuids != null)
                        {
                                    var datas = await monitor.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
                                    foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                              }
                        break;
                        case ActionType.ControlPointCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await control.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.DoorMode:
                        case ActionType.ForceOpenMask:
                        case ActionType.HeldOpenMask:
                        case ActionType.MomentaryUnlock:
                        case ActionType.ReaderLEDModeControl:
                        case ActionType.AbortWaitForDoorOpenState:
                        case ActionType.TempReaderLEDControl:
                        case ActionType.TempDoorMode:
                        case ActionType.HostSimulateCardRead:
                        case ActionType.HostSimulatedKeyPress:
                        if(action.TargetGuids != null)
                        {
                              var datas = await door.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.ProcedureControlCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await repo.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.TriggerVariableControlCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await triggerVar.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
    
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.TimeZoneControl:
                        if(action.TargetGuids != null)
                        {
                              var datas = await time.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
    
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.AntipassbackControlCommand:
                        case ActionType.SetUseLimit:
                        if(action.TargetGuids != null)
                        {
                              var datas = await user.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
    
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.MonitorPointGroupArmDisarmCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await monitorGroup.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
    
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.ModifyAreaConfiguration:
                        if(action.TargetGuids != null)
                        {
                              var datas = await area.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
    
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.TriggerActivationSummary:
                        case ActionType.FilterOutTriggerActivationSummary:
                        if(action.TargetGuids != null)
                        {
                              var datas = await trigger.GetIdAndExternalIdMapGuidByGuidRangeAsync(action.TargetGuids, ct);
    
                              foreach (var data in datas)
                                    {
                                          extractedAction.TryAdd(data.Key, data.Value);
                                    }
                        }
                        break;
                        case ActionType.DelayMinIncrement:
                        case ActionType.DelayMillisecIncrement:
                        case ActionType.DeleteAll:
                        default:
                        break;
                  }
            }

            await adapter.GetAdapter(dev.Vendor).Procedure.DeleteProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            await adapter.GetAdapter(dev.Vendor).Procedure.AddProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  proc.Actions.SelectMany((x,i) => x.TargetGuids == null ? [] : x.TargetGuids.Select(a => (x.Type,x.Metadata,extractedAction[a].externalId,x.Sequence))),
                  ct
            );


            return await Repo.EnableAsync(guid, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetOptionByDeviceAsync(guid,ct);
      }

      public async Task<Guid> UpdateAsync(UpdateProcedureDto dto, CancellationToken ct = default)
      {
            var dev = await repo.GetCommandDetailAsync(dto.Guid, ct);

            // Check name in same location 

            if (await Repo.IsAnyByNameAndLocationIdExceptCurrentAsync(dto.Name,dto.Guid,dev.LocationId ?? 1,ct))
                  throw new DuplicateException(EntityType.PROCEDURE.ToString(), nameof(dto.Name));

            // Extracted 

            Dictionary<ActionType,IEnumerable<(int id,short externalId)>> extractedAction = [];

            foreach(var action in dto.Actions)
            {
                  switch (action.Type)
                  {
                        case ActionType.MonitorPointMask:
                        if(action.TargetGuids != null)
                        {
                              var datas = await monitor.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);

                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.ControlPointCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await control.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.DoorMode:
                        case ActionType.ForceOpenMask:
                        case ActionType.HeldOpenMask:
                        case ActionType.MomentaryUnlock:
                        case ActionType.ReaderLEDModeControl:
                        case ActionType.AbortWaitForDoorOpenState:
                        case ActionType.TempReaderLEDControl:
                        case ActionType.TempDoorMode:
                        case ActionType.HostSimulateCardRead:
                        case ActionType.HostSimulatedKeyPress:
                        if(action.TargetGuids != null)
                        {
                              var datas = await door.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.ProcedureControlCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await repo.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.TriggerVariableControlCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await triggerVar.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.TimeZoneControl:
                        if(action.TargetGuids != null)
                        {
                              var datas = await time.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.AntipassbackControlCommand:
                        case ActionType.SetUseLimit:
                        if(action.TargetGuids != null)
                        {
                              var datas = await user.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.MonitorPointGroupArmDisarmCommand:
                        if(action.TargetGuids != null)
                        {
                              var datas = await monitorGroup.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.ModifyAreaConfiguration:
                        if(action.TargetGuids != null)
                        {
                              var datas = await area.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.TriggerActivationSummary:
                        case ActionType.FilterOutTriggerActivationSummary:
                        if(action.TargetGuids != null)
                        {
                              var datas = await trigger.GetIdsAndExternalIdsByRangeGuidAsync(action.TargetGuids, ct);
    
                              extractedAction.Add(action.Type,datas);
                        }
                        break;
                        case ActionType.DelayMinIncrement:
                        case ActionType.DelayMillisecIncrement:
                        case ActionType.DeleteAll:
                        default:
                        break;
                  }
            }


             var d = new Domain.Entities.Procedure(
                  dto.Guid,
                  dto.Name,
                  dev.DeviceId,
                  dev.LocationId ?? 1,
                  dto.Actions.Select((x,i) => new Domain.Entities.Action(
                        x.Name,
                        x.Type,
                        dev.DeviceId,
                        dev.LocationId ?? 1,
                        (short)i,
                        [.. extractedAction.Where(e => e.Key == x.Type).SelectMany(x => x.Value)],
                        x.Metadata
                  )),
                  dev.ComponentExternalId ?? -1
            );

            // Send command to controller 

            await adapter.GetAdapter(dev.Vendor).Procedure.DeleteProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            await adapter.GetAdapter(dev.Vendor).Procedure.AddProcedureAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  d.Actions.SelectMany(x => x.ActionTarget.Select(a => (x.Type,x.Metadata,a.ExternalId,x.Sequence))),
                  ct
            );

            await repo.UpdateAsync(d, ct);

            return d.Guid;
      }
}