using System;
using Adapter.Abstraction.Command;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Events;
using Adapter.Abstraction.Interfaces;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Interfaces;
using Location.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Device.Application.Behaviors;

public sealed class DeviceBehaviors(IDeviceRepository repo,IMessageBus bus,IAdapterFactory adapterFactory) : IDevice
{
      public async Task<BaseResponse> AsciiCommandAsync(int id,string command, CancellationToken ct = default)
      {
            var Mac = await repo.GetMacByIdAsync(id);
            var ScpId = await repo.GetComponentIdByMacAsync(Mac);
            await adapterFactory.GetAdapter(Venders.AERO).Device.AsciiCommandAsync(Mac,ScpId,command);
             return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }

      public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto,CancellationToken ct=default)
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

            var res = await repo.CreateAsync(device,ct);

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
            
            // TODO: Map domain to dto using AutoMapper or similar library
            await adapterFactory.GetAdapter(Venders.AERO).Device.CreateDeviceAsync(res);

            return res;
      }

      public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto,CancellationToken ct = default)
      {
            var deviceId = await repo.GetIdByMacAsync(dto.Mac);
            var module = new Device.Domain.Entities.Module(
                  0,
                  (short)await repo.GetLowestModuleComponentIdByDeviceIdAsync(deviceId,ct),
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
            var res = await repo.CreateModuleAsync(module,ct);

            dto.Module_id = res.Id;
            dto.Mac = res.Mac;

            await adapterFactory.GetAdapter(Venders.AERO).Device.CreateModuleAsync(
                  res.Mac,
                  (short)res.DeviceComponentId,
                  res.ComponentId,
                  dto.Model,
                  res.Address,
                  res.Port
            );

            return res;
      }

      public async Task<IEnumerable<OptionDto>> GetOptionByLocationIdAsync(int locationId,CancellationToken ct = default)
      {
            // Check if locationId is Exists 
            var flag = await bus.QueryAsync(new IsAnyLocationByIdQuery(locationId));
            if(!flag)
                  throw new BadRequestException(MessageHelper.Location.LocationNotFound);

            var res = await repo.GetOptionByLocationIdAsync(locationId,ct);
            return res;
            
      }

      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            var adapter = adapterFactory.GetAdapter(Venders.AERO);
            return await adapter.Device.GetIdReportsAsync();
      }

      public async Task<List<ModuleDto>> GetModuleByDeviceIdAsync(int id,CancellationToken ct=default)
      {
            return await repo.GetModuleByDeviceIdAsync(id,ct);
            
      }

      public async Task<BaseResponse> GetModuleStatusByIdAsync(int id, CancellationToken ct = default)
      {
            ModuleDto module = await repo.GetModuleByIdAsync(id,ct);
            await bus.SendAsync(new ModuleStatusCommand(module.DeviceComponentId,module.Mac,module.ComponentId));
            return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct=default)
      {
           return await repo.GetPaginationAsync(param,ct);
      }

      public async Task<DeviceStatusDto> GetStatusByIdAsync(int id,CancellationToken ct=default)
      {
            var ComponentId = await repo.GetComponentIdByIdAsync(id,ct);
            var res = await adapterFactory.GetAdapter(Venders.AERO).Device.GetDeviceStatusAsync(ComponentId);
            return new DeviceStatusDto(id, res);
      }

      public async Task<BaseResponse> ResetDeviceAsync(int id,CancellationToken ct =default)
      {
            var Mac = await repo.GetMacByIdAsync(id,ct);
            var ScpId = await repo.GetComponentIdByIdAsync(id,ct);
            await adapterFactory.GetAdapter(Venders.AERO).Device.ResetDeviceAsync(Mac,ScpId);

            return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }

      public async Task<IEnumerable<OptionDto>> GetModuleOptionByDeviceIdAsync(int deviceId, CancellationToken ct = default)
      {
            // Check that ModuleId is Exists
            var flag = await repo.IsAnyModuleByIdAsync(deviceId);
            if(!flag)
                  throw new BadRequestException(MessageHelper.Device.DeviceIdNotFound(deviceId));

            var res = await repo.GetModuleOptionByDeviceIdAsync(deviceId,ct);
            return res;
      }
}
