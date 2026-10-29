using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.MonitorGroup;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class MonitorGroupService(
      IMonitorGroupRepository repo,
      IDeviceRepository device,
      IInputRepository input,
      IDoorRepository door,
      IAdapterFactory adapter
) : BaseService<MonitorGroupDto,MonitorGroup>(repo),IMonitorGroup
{
      public Task<bool> ArmAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<Guid> CreateAsync(CreateMonitorGroupDto dto, CancellationToken ct = default)
      {
            Dictionary<Guid,(int id,short externalId)> monitors = [];
            Dictionary<Guid,(int id,short externalId)> doors = [];
            if(await Repo.IsAnyByNameAsync(dto.Name,ct))
                  throw new DuplicateException(nameof(dto.Name));

            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid,ct);

            if(dto.Components.Any(x => x.Type == SharedKernel.Enums.MonitorType.MONITOR_POINT))
            {
                  monitors = await input.GetIdAndExternalIdMapGuidByGuidRangeAsync(dto.Components.Select(x => x.ComponentGuid),ct);
            }

            if(dto.Components.Any(x => x.Type == SharedKernel.Enums.MonitorType.FORCED_OPEN || x.Type == SharedKernel.Enums.MonitorType.HELD_OPEN))
            {
                  doors = await door.GetIdAndExternalIdMapGuidByGuidRangeAsync(dto.Components.Select(x => x.ComponentGuid),ct);
            }

            var mpgId = await repo.GetFreeExternalIdByMacAsync(dev.DeviceMac,100,[],ct);
                  
            var d = new MonitorGroup(
                  dto.Name,
                  [.. dto.Components.Select(x => new MonitorGroupComponent(
                        x.Type,
                        x.Type == MonitorType.MONITOR_POINT ? monitors[x.ComponentGuid].id : doors[x.ComponentGuid].id
                  ))],
                  dev.DeviceId,
                  mpgId
            );

            // Command
            await adapter.GetAdapter(dev.Vendor).MonitorGroup.CreateMonitorGroupAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  mpgId,
                  dto.Components.Select(x => (x.Type,
                  x.Type == MonitorType.MONITOR_POINT ? monitors[x.ComponentGuid].externalId : doors[x.ComponentGuid].externalId 
                  )).ToList(),
                  ct
            );

            await Repo.AddAsync(d,ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> DisarmAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }


      public async Task<Guid> UpdateAsync(UpdateMonitorGroupDto dto, CancellationToken ct = default)
      {
            Dictionary<Guid,(int id,short externalId)> monitors = [];
            Dictionary<Guid,(int id,short externalId)> doors = [];
            if(await Repo.IsAnyByNameAsync(dto.Name,ct))
                  throw new DuplicateException(nameof(dto.Name));

            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid,ct);

            if(dto.Components.Any(x => x.Type == SharedKernel.Enums.MonitorType.MONITOR_POINT))
            {
                  monitors = await input.GetIdAndExternalIdMapGuidByGuidRangeAsync(dto.Components.Select(x => x.ComponentGuid),ct);
            }

            if(dto.Components.Any(x => x.Type == SharedKernel.Enums.MonitorType.FORCED_OPEN || x.Type == SharedKernel.Enums.MonitorType.HELD_OPEN))
            {
                  doors = await door.GetIdAndExternalIdMapGuidByGuidRangeAsync(dto.Components.Select(x => x.ComponentGuid),ct);
            }

            var mpgId = await repo.GetFreeExternalIdByMacAsync(dev.DeviceMac,100,[],ct);
                  
            var d = new MonitorGroup(
                  dto.Name,
                  [.. dto.Components.Select(x => new MonitorGroupComponent(
                        x.Type,
                        x.Type == MonitorType.MONITOR_POINT ? monitors[x.ComponentGuid].id : doors[x.ComponentGuid].id
                  ))],
                  dev.DeviceId,
                  mpgId
            );

            // Command
            await adapter.GetAdapter(dev.Vendor).MonitorGroup.CreateMonitorGroupAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  mpgId,
                  dto.Components.Select(x => (x.Type,
                  x.Type == MonitorType.MONITOR_POINT ? monitors[x.ComponentGuid].externalId : doors[x.ComponentGuid].externalId 
                  )).ToList(),
                  ct
            );

            await Repo.AddAsync(d,ct);

            return d.Guid;
      }
}