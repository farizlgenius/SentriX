using System;
using System.Net;
using System.Text.Json;
using Adapter.Abstraction.Command;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Events;
using Adapter.Abstraction.Interfaces;
using Device.Application.Interfaces;
using Device.Contract.Command;
using Device.Contract.DTOs;
using Device.Contract.Interfaces;
using Door.Contract.Queries;
using Group.Contract.Queries;
using Input.Contract.Queries;
using Location.Contract.Queries;
using Output.Contract.Queries;
using Setting.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using SharedKernel.Model;
using Time.Contract.Queries;
using User.Contract.Queries;

namespace Device.Application.Behaviors;

public sealed class DeviceBehaviors(IDeviceRepository repo, IMessageBus bus, IAdapterFactory adapterFactory) : IDevice
{
      public async Task AsciiCommandAsync(Guid guid, AeroCommandDto command, CancellationToken ct = default)
      {
            await adapterFactory.GetAdapter(Venders.AERO).Device.AsciiCommandAsync(guid, command.Command);
      }

      public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default)
      {

            // Check that Mac is already exists 
            if (await repo.IsAnyWithMacAsync(StringHelper.FormatMac(dto.Mac)))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Mac)));


            var guid = Guid.NewGuid();
            var domain = new Device.Domain.Entities.Devices(
                  guid,
                  dto.Name,
                  dto.SerialNumber,
                  StringHelper.FormatMac(dto.Mac),
                  dto.Ip,
                  dto.Port,
                  dto.Fw,
                  dto.Type,
                  dto.Status,
                  dto.SyncedAt,
                  dto.LocationId,
                  dto.Metadata,
                  dto.IsActive,
                  dto.IsDefault);

            // TODO: Map domain to dto using AutoMapper or similar library
            await adapterFactory.GetAdapter(dto.Type).Device.CreateDeviceAsync(
                  domain.Guid,
                  domain.Ip,
                  domain.Mac,
                  dto.ScpId,
                  domain.LocationId
                  );

            await repo.AddAsync(domain, ct);

            if (dto.Type.Equals(DeviceType.aero.ToString()))
            {
                  var module = new Device.Domain.Entities.Module(
                 Guid.NewGuid(),
                 0,
                 $"{SioModel.x1100.ToString()} ({0})",
                 dto.SerialNumber,
                 dto.Fw,
                 dto.Port,
                 0,
                 dto.Mac,
                 SioModel.x1100.ToString(),
                 DeviceType.aero.ToString(),
                guid,
                 dto.LocationId,
                 dto.IsActive,
                 dto.IsDefault
                 );

                  await repo.AddModuleAsync(module);
            }
            else if (dto.Type.Equals(DeviceType.amico.ToString()))
            {
                  var module = new Device.Domain.Entities.Module(
                 Guid.NewGuid(),
                 -1,
                 dto.Name,
                 dto.SerialNumber,
                 dto.Fw,
                 dto.Port,
                 -1,
                 dto.Mac,
                 "Amico",
                 DeviceType.amico.ToString(),
                guid,
                 dto.LocationId,
                 dto.IsActive,
                 dto.IsDefault
                 );

                  await repo.AddModuleAsync(module);
            }



            return new DeviceDto(
                  guid,
                  domain.Name,
                  domain.SerialNumber,
                  domain.Mac,
                  domain.Ip,
                  domain.Port,
                  domain.Fw,
                  domain.Type,
                  domain.Status,
                  domain.SyncedAt,
                  domain.LocationId,
                  domain.Metadata,
                  domain.IsActive,
                  domain.IsDefault
            );
      }

      public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default)
      {
            var guid = Guid.NewGuid();
            var d = new Device.Domain.Entities.Module(
                  guid,
                  (short)await repo.GetLowestModuleComponentIdByDeviceGuidAsync(dto.DeviceGuid, ct),
                  $"{((SioModel)dto.Model).ToString()}",
                  string.Empty,
                  string.Empty,
                  dto.Port,
                  dto.Address,
                  string.Empty,
                  dto.Model.ToString(),
                  DeviceType.aero.ToString(),
                  dto.DeviceGuid,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );


            await adapterFactory.GetAdapter(dto.Type).Device.CreateModuleAsync(
                  dto.DeviceGuid,
                  guid,
                 dto.Model,
                 dto.Address,
                 dto.Port
           );


            await repo.AddModuleAsync(d, ct);

            return new ModuleDto(
                  d.Guid,
                  d.Name,
                  d.Fw,
                  d.SerialNumber,
                  (short)d.Port,
                  (short)d.Address,
                  d.Mac,
                  d.Model,
                  d.Type,
                  d.Device_Guid,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByTypeAndLocationIdAsync(int locationId, string type, CancellationToken ct = default)
      {
            // Check if locationId is Exists 
            var flag = await bus.QueryAsync(new IsAnyLocationByIdQuery(locationId));
            if (!flag)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Location", locationId));

            if (string.IsNullOrWhiteSpace(type))
                  throw new BadRequestException(MessageHelper.Common.Empty(type));

            if (!type.Equals(DeviceType.aero.ToString()) && !type.Equals(DeviceType.amico.ToString()))
                  throw new BadRequestException(MessageHelper.Common.NotFound("Type", type));

            if (type.Equals(DeviceType.aero.ToString()))
            {
                  var res = await repo.GetOptionByLocationIdTypeAeroAsync(locationId, type, ct);
                  return res;
            }
            else if (type.Equals(DeviceType.amico.ToString()))
            {
                  var res = await repo.GetOptionByLocationIdTypeAmicoAsync(locationId, type, ct);
                  return res;
            }

            return new List<OptionDto>();


      }

      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            var adapter = adapterFactory.GetAdapter(Venders.AERO);
            return await adapter.Device.GetIdReportsAsync();
      }

      public async Task<IEnumerable<ModuleDto>> GetModuleByDeviceGuidAsync(Guid guid, CancellationToken ct = default)
      {
            // Check that device is exists
            return await repo.GetModuleByDeviceGuidAsync(guid, ct);

      }

      public async Task GetModuleStatusByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            await bus.SendAsync(new ModuleStatusCommand(guid));

      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param, ct);
      }

      public async Task<DeviceStatusDto> GetStatusByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var detail = await repo.GetDeviceByGuidAsync(guid, ct);
            var res = await adapterFactory.GetAdapter(detail.Type).Device.GetDeviceStatusAsync(detail.Guid);
            return new DeviceStatusDto(guid, res);
      }

      public async Task ResetDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            var detail = await repo.GetDeviceByGuidAsync(guid, ct);
            await adapterFactory.GetAdapter(detail.Type).Device.ResetDeviceAsync(detail.Guid);

      }

      public async Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceGuidAsync(Guid guid, CancellationToken ct = default)
      {
            var res = await repo.GetModuleOptionByDeviceGuidAsync(guid, ct);
            return res;
      }


      public async Task<IEnumerable<OptionDto>> GetReaderOptionsByModuleGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetReaderOptionsByModuleGuidAsync(guid, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetInputOptionsByModuleIdAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetInputOptionsByModuleIdAsync(guid, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetRelayOptionsByModuleIdAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetRelayOptionsByModuleIdAsync(guid, ct);
      }

      public async Task GetEventStatusAsync(Guid guid, CancellationToken ct = default)
      {
            var detail = await repo.GetMacAndTypeAndComponentIdByGuidAsync(guid, ct);

            await adapterFactory.GetAdapter(detail.Type).Device.GetEventStatusAsync(detail.Mac, detail.ComponentId);

      }

      public async Task SetEventStatusAsync(SetEventDto dto, CancellationToken ct = default)
      {
            var detail = await repo.GetMacAndTypeAndComponentIdByGuidAsync(dto.DeviceGuid, ct);

            await adapterFactory.GetAdapter(detail.Type).Device.SetEventStatusAsync(detail.Mac, detail.ComponentId, dto.IsEnable);


      }

      public async Task<string> GetModuleNameByMacAndComponentIdAsync(string Mac, short ComponentId, CancellationToken ct = default)
      {
            return await repo.GetModuleNameByMacAndComponentIdAsync(Mac, ComponentId, ct);
      }

      public async Task UploadDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            // Device
            var device = await repo.GetDeviceByGuidAsync(guid);
            string Mac = device.Mac.Replace(":", "_");

            if (device.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Device", guid.ToString()));

            // Module // Aero Only
            var modules = await repo.GetModuleByDeviceGuidAsync(device.Guid);

            foreach (var module in modules)
            {
                  if (device.Type.Equals(DeviceType.aero.ToString()))
                  {
                        if (Enum.TryParse<SioModel>(module.Model, out var status))
                        {
                              short enumValue = (short)status;
                              await adapterFactory.GetAdapter(device.Type).Device.CreateModuleAsync(
                              device.Guid,
                              module.Guid,
                              enumValue,
                              module.Address,
                              module.Port
                              );
                        }
                  }


            }

            // Card Format // Aero Only
            var formats = await bus.QueryAsync(new CardFormatByLocationIdQuery(device.LocationId));

            foreach (var cfmt in formats)
            {
                  if (device.Type.Equals(DeviceType.aero.ToString()))
                  {
                        await adapterFactory.GetAdapter(device.Type).Setting.CardFormatConfiguration(
                       Mac,
                        device.ComponentId,
                        cfmt.ComponentId,
                        cfmt.Fac,
                        cfmt.Offset,
                        cfmt.FunctionId,
                        cfmt.Flag,
                        cfmt.Bits,
                        cfmt.PeLn,
                        cfmt.PeLoc,
                        cfmt.PoLn,
                        cfmt.PoLoc,
                        cfmt.FcLn,
                        cfmt.FcLoc,
                        cfmt.ChLn,
                        cfmt.ChLoc,
                        cfmt.IcLn,
                        cfmt.IcLoc
                  );
                  }

            }

            // Input // Aero Only
            var inputs = await bus.QueryAsync(new InputByMacQuery(Mac));

            foreach (var input in inputs)
            {
                  if (device.Type.Equals(DeviceType.aero.ToString()))
                  {
                        await adapterFactory.GetAdapter(device.Type).Monitor.CreateUpdateMonitorPoint(
                        Mac,
                        input.ComponentId,
                        input.DeviceComponentId,
                        input.ModuleComponentId,
                        input.InputNo,
                        input.SensorMode,
                        input.Debounce,
                        input.HoldTime,
                        input.LogFunction,
                        input.LatchMode,
                        input.DelayEntry,
                        input.DelayExit
                  );
                  }

            }



            // Input Group // Aero
            var igps = await bus.QueryAsync(new InputGroupByMacQuery(Mac));

            foreach (var g in igps)
            {
                  if (device.Type.Equals(DeviceType.aero.ToString()))
                  {
                        await adapterFactory.GetAdapter(device.Type).Monitor.CreateUpdateMonitorGroup(
                       Mac,
                       device.ComponentId,
                       g.ComponentId,
                       g.InputGroupDetailDtos.Select(x => (x.InputType, x.InputComponentId)).ToList()
                 );
                  }

            }


            // Output // Aero

            var outputs = await bus.QueryAsync(new OutputByMacQuery(Mac));
            foreach (var o in outputs)
            {
                  if (device.Type.Equals(DeviceType.aero.ToString()))
                  {
                        await adapterFactory.GetAdapter(device.Type).Control.CreateAsync(
                        o.Mac,
                        o.ComponentId,
                        o.DeviceComponentId,
                        o.ModuleComponentId,
                        o.OutputNo,
                        o.DriveMode,
                        o.OfflineMode,
                        o.DefaultPulse
                  );
                  }

            }

            // TimeZone
            var timeZones = await bus.QueryAsync(new TimeZoneByLocationIdQuery(device.LocationId));

            foreach (var time in timeZones)
            {

                  await adapterFactory.GetAdapter(device.Type).Time.CreateTimeZoneAsync(
                        time.Guid,
                         device.ComponentId,
                        time.ComponentId,
                        time.Name,
                        device.Mac,
                        time.Intervals.Select(x => new IntervalObject(
                              (short)x.ComponentId,
                              DateTimeHelper.ConvertTimeToEndMinute(x.Start),
                              DateTimeHelper.ConvertTimeToEndMinute(x.End),
                              x.Days.Sunday,
                              x.Days.Monday,
                              x.Days.Tuesday,
                              x.Days.Wednesday,
                              x.Days.Thursday,
                              x.Days.Friday,
                              x.Days.Saturday
                        )).ToList()
                  );
            }

            // Doors
            var doors = await bus.QueryAsync(new DoorByMacQuery(Mac));

            foreach (var door in doors)
            {
                  await adapterFactory.GetAdapter(device.Type).Door.CreateUpdateDoorAsync(
                        Mac,
                        device.ComponentId,
                        door.Metadata,
                        door.ComponentId,
                        door.SecondComponentId
                  );
            }

            // Access Level
            var groups = await bus.QueryAsync(new GroupByMacAndDeviceTypeQuery(Mac, device.Type));

            foreach (var g in groups)
            {
                  var door = g.Doors.Select(x => new
                  {
                        x.Mac,
                        x.DeviceComponentId,
                        x.DoorComponentId,
                        x.TimezoneComponentId
                  }).ToList();
                  await adapterFactory.GetAdapter(device.Type).Group.CreateGroup(
                        g.Name,
                        g.ComponentId,
                        door.Select(x => (x.Mac,x.DeviceComponentId,x.DoorComponentId,x.TimezoneComponentId)).ToList()
                  );
            }

            // Users
            var gpGuidList = await bus.QueryAsync(new GroupGuidsByMacQuery(Mac));

            var users = await bus.QueryAsync(new UsersByGroupGuidsQuery(gpGuidList.Select(x => x.guid).ToList()));

            foreach (var user in users)
            {
                  await adapterFactory.GetAdapter(device.Type).User.AddUserAsync(
                        Mac,
                        device.ComponentId,
                        user.Identification,
                        $"{user.Title} {user.FirstName} {user.MiddleName} ${user.LastName}",
                       (int)DateTimeHelper.DateTimeToElapeSecond(user.ActiveTime),
                       (int)DateTimeHelper.DateTimeToElapeSecond(user.ExpireTime),
                        user.Card.CardNumber,
                        user.LicensePlate.LicensePlate,
                        user.Pin.Pin,
                        user.QrCode.QrCode,
                        user.Face.ImageName,
                        gpGuidList.Select(x => x.componentId).ToList()
                  );
            }

            // Area


            // Trigger

            // Send Command Update sync datetime 
            await bus.SendAsync(new DeviceSyncTimeCommand(Mac));

            // Check again
            await adapterFactory.GetAdapter(device.Type).Device.VerifyDeviceComponentAsync(device.ComponentId);



      }

      public async Task<JsonElement> GetAmicoDeviceInformationAsync(AmicoStartSessionDto dto)
      {
            var res = await adapterFactory.GetAdapter(DeviceType.amico.ToString()).Device.GetDeviceInformationByIpAsync(dto.Ip, true);

            return res;
      }

      public async Task<DeviceDto> DeleteDeviceAsync(Guid guid, CancellationToken ct = default)
      {
            var d = await repo.GetDeviceByGuidAsync(guid);

            if(d.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(guid),guid.ToString()));

            await adapterFactory.GetAdapter(d.Type).Device.DeleteDeviceAsync(guid,d.Ip,StringHelper.FormatMac(d.Mac),d.ComponentId);

            await repo.DeleteAsync(guid);

            return new DeviceDto(
                  d.Guid,
                  d.Name,
                  d.SerialNumber,
                  d.Mac,
                  d.Ip,
                  d.Port,
                  d.Fw,
                  d.Type,
                  d.Status,
                  d.SyncedAt,
                  d.LocationId,
                  d.Metadata,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<DeviceDto> GetDeviceByDeviceIdAsync(string DeviceId, CancellationToken ct = default)
      {
            return await repo.GetDeviceByDeviceIdAsync(DeviceId);
      }

      public async Task<DeviceDto> GetDeviceByGuidAsync(Guid guid, CancellationToken ct = default)
      {
            return await repo.GetDeviceByGuidAsync(guid);
      }
}
