using System;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Events;
using Adapter.Abstraction.Interfaces;
using Adapter.Abstraction.Query;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Device.Application.Behaviors;

public sealed class DeviceBehaviors(IDeviceRepository repo,IMessageBus bus,IAdapterFactory adapterFactory) : IDevice
{
      public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto,CancellationToken ct=default)
      {
            var device = new Device.Domain.Entities.Devices(0, dto.Name, dto.SerialNumber, dto.Mac, dto.Ip, dto.Port, dto.Fw, dto.Type, dto.Status, dto.SyncedAt, dto.LocationId,dto.Metadata);
            var res = await repo.CreateAsync(device,ct);
            
            // TODO: Map domain to dto using AutoMapper or similar library
            await adapterFactory.GetAdapter(Venders.AERO).Device.CreateDeviceAsync(dto);

            return res;
      }

      public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto,CancellationToken ct = default)
      {
            var deviceId = await repo.GetIdByMacAsync(dto.Mac);
            var module = new Device.Domain.Entities.Module(
                  $"{((SioModel)dto.Model).ToString()}",
                  string.Empty,
                  string.Empty,
                  dto.Port,
                  dto.Address,
                  string.Empty,
                  dto.Model.ToString(),
                  deviceId
                  );
            var res = await repo.CreateModuleAsync(module,ct);

            dto.Module_id = res.Id;

            await adapterFactory.GetAdapter(Venders.AERO).Device.CreateModuleAsync(dto);

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
            var Mac = await repo.GetMacByIdAsync(module.DeviceId,ct);
            await bus.PublishAsync(new ModuleStatusByDeviceIdEvent(Mac,module.Id));
            return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }

      public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParams param,CancellationToken ct=default)
      {
           return await repo.GetPaginationAsync(param,ct);
      }

      public async Task<DeviceStatusDto> GetStatusByIdAsync(int id,CancellationToken ct=default)
      {
            var Mac = await repo.GetMacByIdAsync(id,ct);
            var res = await adapterFactory.GetAdapter(Venders.AERO).Device.GetDeviceStatusByMacAsync(Mac);
            return new DeviceStatusDto(id, res);
      }

      public async Task<BaseResponse> ResetDeviceAsync(int id,CancellationToken ct =default)
      {
            var Mac = await repo.GetMacByIdAsync(id,ct);
            await adapterFactory.GetAdapter(Venders.AERO).Device.ResetDeviceAsync(Mac);

            return new BaseResponse(System.Net.HttpStatusCode.OK,MessageHelper.Common.Success,DateTime.UtcNow);
      }


}
