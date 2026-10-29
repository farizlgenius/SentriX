using Adapter.Contract.Interfaces;
using Core.Application.Interfaces;
using Core.Contract.DTOs.Area;
using Core.Contract.Interfaces;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace Core.Application.Services;

public sealed class AreaService(
      IAreaRepository repo,
      IDoorRepository door,
      IDeviceRepository device,
      IDeviceModuleRepository module,
      IInputRepository input,
      IOutputRepository output,
      ITimeRepository time,
      IAdapterFactory adapter
      ) : BaseService<AreaDto, Domain.Entities.Area>(
      repo
), IArea
{
      public async Task<Guid> CreateAsync(CreateAreaDto dto, CancellationToken ct = default)
      {
            var dev = await device.GetCommandDetailAsync(dto.DeviceGuid, ct);

            var areaId = await Repo.GetFreeExeternalIdAsync(100, [], ct);

            var doorIds = await door.GetDoorIdsMapGuidsAsync(dto.Doors.Select(x => x.DoorGuid), ct);

            var areaFromId = await repo.GetAreaIdMapGuidByGuidsAsync(dto.Doors.Select(x => x.AreaFromGuid), ct);

            var areaFromExternalId = await repo.GetExternalIdMapGuidByGuidRangeAsync(dto.Doors.Select(x => x.AreaFromGuid),ct);

            // Area To is this area

            var d = new Area(
                  dto.Name,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  [.. dto.Doors.Select(x => (doorIds[x.DoorGuid], x.AntipassbackFrom, areaFromId[x.AreaFromGuid], DoorDirection.IN, x.Type))],
                  [.. dto.Doors.Where(x => x.Type == DoorType.Dual).Select(x => (doorIds[x.DoorGuid], x.AntipassbackTo, 0, DoorDirection.OUT, x.Type))],
                  areaId,
                  dev.DeviceId
            );

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
            foreach (var dor in dto.Doors)
            {
                  var de = await door.GetCommandDetailAsync(dor.DoorGuid, ct);

                  var doorDto = await door.GetAsync(dor.DoorGuid, ct);

                  var readerModuleId = await module.GetExternalIdMapGuidByGuidRangeAsync(doorDto.Readers.Select(x => x.DeviceModuleGuid), ct);

                  var rexesModuleId = await module.GetExternalIdMapGuidByGuidRangeAsync(doorDto.Rexes.Select(x => x.DeviceModuleGuid), ct);

                  var timeExternalId = await time.GetExternalIdMapGuidByGuidRangeAsync(doorDto.Rexes.Select(x => x.MaskGuid ?? Guid.Empty), ct);

                  await adapter.GetAdapter(de.Vendor).Door.AddDoorsAsync(
                        de.DeviceMac,
                        de.DeviceIp,
                        doorDto.Type,
                        de.DeviceExternalId,
                        [de.ComponentExternalId ?? -1, de.AltrComponentExternalId ?? -1],
                        doorDto.Metadata,
                        [.. doorDto.Readers.Select(x => ((short)x.SlotNo, x.Mode, x.ReaderDirection, x.Metadata, readerModuleId[x.DeviceModuleGuid]))],
                        doorDto.Buzzer == null ? null : ((short)doorDto.Buzzer.SlotNo, doorDto.Buzzer.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Buzzer.DeviceModuleGuid, ct), await output.GetExternalIdByGuidAsync(doorDto.Buzzer.Guid ?? Guid.Empty, ct)),
                        [.. doorDto.Rexes.Select(x => ((short)x.SlotNo, x.Metadata, rexesModuleId[x.DeviceModuleGuid], x.MaskGuid == Guid.Empty || x.MaskGuid == null ? (short)0 : timeExternalId[x.MaskGuid ?? Guid.Empty]))],
                        doorDto.Bg == null ? null : ((short)doorDto.Bg.SlotNo, doorDto.Bg.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Bg.DeviceModuleGuid, ct), await input.GetExternalIdByGuidAsync(doorDto.Bg.Guid ?? Guid.Empty, ct)),
                        doorDto.Sensor == null ? null : ((short)doorDto.Sensor.SlotNo, doorDto.Sensor.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Sensor.DeviceModuleGuid, ct)),
                        doorDto.Relay == null ? null : ((short)doorDto.Relay.SlotNo, doorDto.Relay.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Relay.DeviceModuleGuid, ct)),
                        areaFromExternalId[dor.AreaFromGuid],
                        areaId,
                        dor.AntipassbackFrom,
                        dor.AntipassbackTo,
                        ct
                  );
            }



            await Repo.AddAsync(d, ct);

            return d.Guid;
      }

      public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid, ct);

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.EnabledAreaAsync(
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
            throw new NotImplementedException();
      }

      public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid, ct);

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.EnabledAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );


            await Repo.DisableAsync(guid, ct);

            return true;
      }

      public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
      {
            var dev = await Repo.GetCommandDetailAsync(guid, ct);

            // Command 
            await adapter.GetAdapter(dev.Vendor).Area.EnabledAreaAsync(
                  dev.DeviceMac,
                  dev.DeviceIp,
                  dev.DeviceExternalId,
                  dev.ComponentExternalId ?? -1,
                  ct
            );


            await Repo.EnableAsync(guid, ct);

            return true;
      }

      public async Task<bool> GetStatusAsync(Guid guid, CancellationToken ct = default)
      {
           var dev = await repo.GetCommandDetailAsync(guid,ct);

           await adapter.GetAdapter(dev.Vendor).Area.GetStatusAsync(
            dev.DeviceMac,
            dev.DeviceIp,
            dev.DeviceExternalId,
            dev.ComponentExternalId ?? -1,
            ct
           );

           return true;

      }

      public async Task<Guid> UpdateAsync(UpdateAreaDto dto, CancellationToken ct = default)
      {
            var dev = await repo.GetCommandDetailAsync(dto.Guid, ct);

            var doors = await door.GetExternalIdMapGuidByGuidRangeAsync(dto.Doors.Select(x => x.DoorGuid), ct);

            var areaFrom = await Repo.GetExternalIdMapGuidByGuidRangeAsync(dto.Doors.Select(x => x.AreaFromGuid), ct);

            var areaTo = await Repo.GetExternalIdMapGuidByGuidRangeAsync(dto.Doors.Select(x => x.AreaToGuid ?? Guid.Empty), ct);

            var d = new Area(
                  dto.Name,
                  dto.MultiMode,
                  dto.Mode,
                  dto.OccMax,
                  dto.OccDown,
                  dto.OccUp,
                  dto.Metadata,
                  [.. dto.Doors.Select(x => (doors[x.DoorGuid], x.AntipassbackFrom, areaFrom[x.AreaFromGuid], DoorDirection.IN, x.Type))],
                  [.. dto.Doors.Where(x => x.Type == DoorType.Dual).Select(x => (doors[x.DoorGuid], x.AntipassbackTo, areaTo[x.AreaToGuid ?? Guid.Empty], DoorDirection.OUT, x.Type))],
                  dev.ComponentExternalId ?? -1,
                  dev.DeviceId
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



            // Send Door Command 
            foreach (var dor in dto.Doors)
            {
                  var de = await door.GetCommandDetailAsync(dor.DoorGuid, ct);

                  var doorDto = await door.GetAsync(dor.DoorGuid, ct);

                  var readerModuleId = await module.GetExternalIdMapGuidByGuidRangeAsync(doorDto.Readers.Select(x => x.DeviceModuleGuid), ct);

                  var rexesModuleId = await module.GetExternalIdMapGuidByGuidRangeAsync(doorDto.Rexes.Select(x => x.DeviceModuleGuid), ct);

                  var timeExternalId = await time.GetExternalIdMapGuidByGuidRangeAsync(doorDto.Rexes.Select(x => x.MaskGuid ?? Guid.Empty), ct);

                  await adapter.GetAdapter(de.Vendor).Door.AddDoorsAsync(
                        de.DeviceMac,
                        de.DeviceIp,
                        doorDto.Type,
                        de.DeviceExternalId,
                        [de.ComponentExternalId ?? -1, de.AltrComponentExternalId ?? -1],
                        doorDto.Metadata,
                        [.. doorDto.Readers.Select(x => ((short)x.SlotNo, x.Mode, x.ReaderDirection, x.Metadata, readerModuleId[x.DeviceModuleGuid]))],
                        doorDto.Buzzer == null ? null : ((short)doorDto.Buzzer.SlotNo, doorDto.Buzzer.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Buzzer.DeviceModuleGuid, ct), await output.GetExternalIdByGuidAsync(doorDto.Buzzer.Guid ?? Guid.Empty, ct)),
                        [.. doorDto.Rexes.Select(x => ((short)x.SlotNo, x.Metadata, rexesModuleId[x.DeviceModuleGuid], timeExternalId[x.MaskGuid ?? Guid.Empty]))],
                        doorDto.Bg == null ? null : ((short)doorDto.Bg.SlotNo, doorDto.Bg.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Bg.DeviceModuleGuid, ct), await input.GetExternalIdByGuidAsync(doorDto.Bg.Guid ?? Guid.Empty, ct)),
                        doorDto.Sensor == null ? null : ((short)doorDto.Sensor.SlotNo, doorDto.Sensor.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Sensor.DeviceModuleGuid, ct)),
                        doorDto.Relay == null ? null : ((short)doorDto.Relay.SlotNo, doorDto.Relay.Metadata, await module.GetExternalIdByGuidAsync(doorDto.Relay.DeviceModuleGuid, ct)),
                        areaFrom[dor.AreaFromGuid],
                        de.ComponentExternalId ?? -1,
                        dor.AntipassbackFrom,
                        dor.AntipassbackTo,
                        ct
                  );
            }


            await Repo.UpdateAsync(d, ct);

            return d.Guid;
      }
}