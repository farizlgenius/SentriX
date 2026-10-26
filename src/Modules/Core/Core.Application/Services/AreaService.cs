using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Area;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class AreaService(
      IAreaRepository repo,
      IDoorRepository door,
      IDeviceRepository device,
      IAdapterFactory adapter
      ) : BaseService<AreaDto,Domain.Entities.Area>(
      repo
),IArea
{
      public async Task<Guid> CreateAsync(CreateAreaDto dto, CancellationToken ct = default)
      {

            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid,ct);

            var areaId = await repo.GetFreeExeternalIdAsync(100,[],ct);

            var doorIn = await door.GetExternalIdMapGuidByGuidRangeAsync(dto.AreaInDoors.Select(x => x.DoorGuid),ct);

            var doorOut = await door.GetExternalIdMapGuidByGuidRangeAsync(dto.AreaOutDoors.Select(x => x.DoorGuid),ct);


            var d = new Area(
                  dto.Name,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  [.. dto.AreaInDoors.Select(x => (doorIn[x.DoorGuid],x.Antipassback))],
                  [.. dto.AreaOutDoors.Select(x => (doorOut[x.DoorGuid],x.Antipassback))],
                  dev.DeviceId,
                  areaId
            );

            // Send Area

            await adapter.GetAdapter(dev.Vendor).Area.AddAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  areaId,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  ct
            );

            // Send Door Command 

            await Repo.AddAsync(d,ct);


            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.EnabledAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );

            await Repo.DeleteAsync(guid,ct);

            return true;
      }

      public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.EnabledAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );


            await Repo.DisableAsync(guid,ct);

            return true;
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid,ct);

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.EnabledAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );


            await Repo.EnableAsync(guid,ct);

            return true;
      }

      public async Task<Guid> UpdateAsync(UpdateAreaDto dto, CancellationToken ct = default)
      {
            var dev = await repo.GetCommandDetailAsync(dto.Guid,ct);

            var d = new Area(
                  dto.Guid,
                  dto.Name,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata
            );

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.AddAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  ct
            );


            await Repo.UpdateAsync(d,ct);

            return d.Guid;
      }
}