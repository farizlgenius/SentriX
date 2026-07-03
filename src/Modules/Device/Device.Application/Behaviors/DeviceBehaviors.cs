using System;
using System.Net;
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
using Time.Contract.Queries;
using User.Contract.Queries;

namespace Device.Application.Behaviors;

public sealed class DeviceBehaviors(IDeviceRepository repo, IMessageBus bus, IAdapterFactory adapterFactory) : IDevice
{
      public async Task<BaseResponse> AsciiCommandAsync(int id, AeroCommandDto command, CancellationToken ct = default)
      {
            var Mac = await repo.GetMacByIdAsync(id);
            var ScpId = await repo.GetComponentIdByMacAsync(Mac);
            await adapterFactory.GetAdapter(Venders.AERO).Device.AsciiCommandAsync(Mac, ScpId, command.Command);
            return new BaseResponse(System.Net.HttpStatusCode.OK, MessageHelper.Common.Success, DateTime.UtcNow);
      }

      public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto, CancellationToken ct = default)
      {
            var device = new Device.Domain.Entities.Devices(
                  0,
                  dto.ComponentId,
                  dto.Name,
                  dto.SerialNumber,
                  dto.Mac,
                  dto.Ip,
                  dto.Port,
                  dto.Fw,
                  dto.Type,
                  dto.Status,
                  dto.SyncedAt,
                  dto.LocationId,
                  dto.Metadata,
                  dto.IsActive);

            // TODO: Map domain to dto using AutoMapper or similar library
            await adapterFactory.GetAdapter(dto.Type).Device.CreateDeviceAsync(dto.Mac, dto.ComponentId);

            var res = await repo.CreateAsync(device, ct);

            var module = new Device.Domain.Entities.Module(
                  0,
                  0,
                  $"{SioModel.x1100.ToString()} ({0})",
                  string.Empty,
                  string.Empty,
                  0,
                  0,
                  dto.Mac,
                  SioModel.x1100.ToString(),
                  DeviceType.AERO.ToString(),
                  res.Id,
                  dto.LocationId,
                  dto.IsActive
                  );

            await repo.CreateModuleAsync(module);

            return res;
      }

      public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default)
      {
            var deviceId = await repo.GetIdByMacAsync(dto.Mac);
            var module = new Device.Domain.Entities.Module(
                  0,
                  (short)await repo.GetLowestModuleComponentIdByDeviceIdAsync(deviceId, ct),
                  $"{((SioModel)dto.Model).ToString()}",
                  string.Empty,
                  string.Empty,
                  dto.Port,
                  dto.Address,
                  string.Empty,
                  dto.Model.ToString(),
                  DeviceType.AERO.ToString(),
                  deviceId,
                  dto.LocationId,
                  true
                  );


            await adapterFactory.GetAdapter(dto.Type).Device.CreateModuleAsync(
                 dto.Mac,
                 (short)dto.DeviceComponentId,
                 dto.ComponentId,
                 dto.Model,
                 dto.Address,
                 dto.Port
           );


            var res = await repo.CreateModuleAsync(module, ct);


            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByTypeAndLocationIdAsync(int locationId, string type, CancellationToken ct = default)
      {
            // Check if locationId is Exists 
            var flag = await bus.QueryAsync(new IsAnyLocationByIdQuery(locationId));
            if (!flag)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Location", locationId));

            if (string.IsNullOrWhiteSpace(type))
                  throw new BadRequestException(MessageHelper.Common.Empty(type));

            if (!type.Equals(DeviceType.AERO.ToString()) && !type.Equals(DeviceType.AMICO.ToString()))
                  throw new BadRequestException(MessageHelper.Common.NotFound("Type", type));

            if (type.Equals(DeviceType.AERO.ToString()))
            {
                  var res = await repo.GetOptionByLocationIdTypeAeroAsync(locationId, type, ct);
                  return res;
            }
            else if (type.Equals(DeviceType.AMICO.ToString()))
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

      public async Task<IEnumerable<ModuleDto>> GetModuleByDeviceIdAsync(int id, CancellationToken ct = default)
      {
            return await repo.GetModuleByDeviceIdAsync(id, ct);

      }

      public async Task<BaseResponse> GetModuleStatusByIdAsync(int id, CancellationToken ct = default)
      {
            ModuleDto module = await repo.GetModuleByIdAsync(id, ct);
            await bus.SendAsync(new ModuleStatusCommand(module.DeviceComponentId, module.Mac, module.ComponentId));
            return new BaseResponse(System.Net.HttpStatusCode.OK, MessageHelper.Common.Success, DateTime.UtcNow);
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetPaginationAsync(param, ct);
      }

      public async Task<DeviceStatusDto> GetStatusByIdAsync(int id, CancellationToken ct = default)
      {
            var ComponentId = await repo.GetComponentIdByIdAsync(id, ct);
            var res = await adapterFactory.GetAdapter(Venders.AERO).Device.GetDeviceStatusAsync(ComponentId);
            return new DeviceStatusDto(id, res);
      }

      public async Task<BaseResponse> ResetDeviceAsync(int id, CancellationToken ct = default)
      {
            var Mac = await repo.GetMacByIdAsync(id, ct);
            var ScpId = await repo.GetComponentIdByIdAsync(id, ct);
            await adapterFactory.GetAdapter(Venders.AERO).Device.ResetDeviceAsync(Mac, ScpId);

            return new BaseResponse(System.Net.HttpStatusCode.OK, MessageHelper.Common.Success, DateTime.UtcNow);
      }

      public async Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceIdAsync(int deviceId, CancellationToken ct = default)
      {
            // Check that ModuleId is Exists
            var flag = await repo.IsAnyModuleByIdAsync(deviceId);
            if (!flag)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Module", deviceId));

            var res = await repo.GetModuleOptionByDeviceIdAsync(deviceId, ct);
            return res;
      }

      public async Task<DeviceDto> GetDeviceByComponentIdAsync(int ComponentId, CancellationToken ct = default)
      {
            return await repo.GetDeviceByComponentIdAsync(ComponentId, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetReaderOptionsByModuleIdAsync(int id, CancellationToken ct = default)
      {
            return await repo.GetReaderOptionsByModuleIdAsync(id, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetInputOptionsByModuleIdAsync(int id, CancellationToken ct = default)
      {
            return await repo.GetInputOptionsByModuleIdAsync(id, ct);
      }

      public async Task<IEnumerable<OptionDto>> GetRelayOptionsByModuleIdAsync(int id, CancellationToken ct = default)
      {
            return await repo.GetRelayOptionsByModuleIdAsync(id, ct);
      }

      public async Task<BaseResponse> GetEventStatusAsync(string type, int id, CancellationToken ct = default)
      {
            var mac = await repo.GetMacByIdAsync(id);
            var scpid = await repo.GetComponentIdByIdAsync(id);

            await adapterFactory.GetAdapter(type).Device.GetEventStatusAsync(mac, scpid);

            return new BaseResponse(HttpStatusCode.OK, MessageHelper.Common.Success, DateTime.UtcNow);
      }

      public async Task<BaseResponse> SetEventStatusAsync(SetEventDto dto, CancellationToken ct = default)
      {
            var mac = await repo.GetMacByIdAsync(dto.DeviceId);
            var scpid = await repo.GetComponentIdByIdAsync(dto.DeviceId);

            await adapterFactory.GetAdapter(dto.Type).Device.SetEventStatusAsync(mac, scpid, dto.IsEnable);

            return new BaseResponse(HttpStatusCode.OK, MessageHelper.Common.Success, DateTime.UtcNow);
      }

      public async Task<string> GetModuleNameByMacAndComponentIdAsync(string Mac, short ComponentId, CancellationToken ct = default)
      {
            return await repo.GetModuleNameByMacAndComponentIdAsync(Mac,ComponentId,ct);
      }

      public async Task<BaseResponse> UploadDeviceAsync(int id, CancellationToken ct = default)
      {
           // Device
           var device = await repo.GetDeviceByIdAsync(id);

           if(device.Id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Device",id));

           // Module
           var modules = await repo.GetModuleByDeviceIdAsync(id);

           foreach(var module in modules)
            {
                  if (device.Type.Equals(DeviceType.AERO.ToString()))
                  {
                        if (Enum.TryParse<SioModel>(module.Model, out var status))
                        {
                              short enumValue = (short)status;
                              await adapterFactory.GetAdapter(device.Type).Device.CreateModuleAsync(
                              module.Mac,
                              device.ComponentId,
                              module.ComponentId,
                              enumValue,
                              module.Address,
                              module.Port
                              );
                        }
                  }


            }

            // Card Format
            var formats = await bus.QueryAsync(new CardFormatByLocationIdQuery(device.LocationId));

            foreach(var cfmt in formats)
            {
                  await adapterFactory.GetAdapter(device.Type).Setting.CardFormatConfiguration(
                        device.Mac,
                        device.ComponentId,
                        cfmt.ComponentId,
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

            // Input
            var inputs = await bus.QueryAsync(new InputByMacQuery(device.Mac));

            foreach(var input in inputs)
            {
                  await adapterFactory.GetAdapter(device.Type).Monitor.CreateUpdateMonitorPoint(
                        device.Mac,
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



            // Input Group
            var igps = await bus.QueryAsync(new InputGroupByMacQuery(device.Mac));

            foreach(var g in igps)
            {
                  await adapterFactory.GetAdapter(device.Type).Monitor.CreateUpdateMonitorGroup(
                        device.Mac,
                        device.ComponentId,
                        g.ComponentId,
                        g.InputGroupDetailDtos.Select(x => (x.InputType,x.InputComponentId)).ToList()
                  );
            }
            

            // Output

            var outputs = await bus.QueryAsync(new OutputByMacQuery(device.Mac));
            foreach(var o in outputs)
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

            // TimeZone
            var timeZones = await bus.QueryAsync(new TimeZoneByLocationIdQuery(device.LocationId));

            foreach(var time in timeZones)
            {
                  await adapterFactory.GetAdapter(device.Type).Time.CreateTimezoneAsync(
                        device.Mac,
                        device.ComponentId,
                        time.ComponentId,
                        time.Mode,
                        time.Active,
                        time.Deactive,
                        time.Intervals
                  );
            }

            // Doors
            var doors = await bus.QueryAsync(new DoorByMacQuery(device.Mac));

            foreach(var door in doors)
            {
                  await adapterFactory.GetAdapter(device.Type).Door.CreateUpdateDoorAsync(
                        device.Mac,
                        device.ComponentId,
                        door.Metadata,
                        door.ComponentId,
                        door.SecondComponentId
                  );
            }

            // Access Level
            var groups = await bus.QueryAsync(new GroupByMacAndDeviceTypeQuery(device.Mac,device.Type));

            foreach(var group in groups)
            {
                  await adapterFactory.GetAdapter(device.Type).Group.CreateUpdateLevel(
                        device.Mac,
                        device.ComponentId,
                        group.ComponentId,
                        group.Doors.Select(x => (x.DoorComponentId,x.TimezoneComponentId)).ToList()
                  );
            }

            // Users
            var gpList = await bus.QueryAsync(new GroupIdListByMacQuery(device.Mac));
            var creds = await bus.QueryAsync(new CredentialByGroupListQuery(gpList.Select(x => x.id).ToList()));

            foreach(var cred in creds)
            {
                  await adapterFactory.GetAdapter(device.Type).User.CreateUserAsync(
                        device.Mac,
                        device.ComponentId,
                        cred.Flag,
                        cred.CardNumber,
                        cred.IssueCode,
                        cred.Pin,
                        gpList.Select(x => x.componentId).ToList(),
                        cred.ApbLoc,
                        cred.UseCount,
                        (short)DateTimeHelper.DateTimeToElapeSecond(cred.Active),
                        (short)DateTimeHelper.DateTimeToElapeSecond(cred.Expire),
                        -1,
                        -1,
                        -1,
                        -1
                  );
            }

            // Area


            // Trigger

            // Send Command Update sync datetime 
            await bus.SendAsync(new DeviceSyncTimeCommand(device.Mac));

            
            return new BaseResponse(
                  HttpStatusCode.OK,
                  MessageHelper.Common.Success,
                  DateTime.UtcNow
                  );
           
      }

      public async Task<string> GetAmicoDeviceInformationAsync(AmicoStartSessionDto dto)
      {
            var res = await adapterFactory.GetAdapter(DeviceType.AMICO.ToString()).Device.GetDeviceInformationAsync(
                  dto.Ip,
                  dto.Login,
                  dto.Password);

            return res;
      }
}
