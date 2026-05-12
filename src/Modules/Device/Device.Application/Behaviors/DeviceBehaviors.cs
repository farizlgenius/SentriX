using System;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Application.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Interfaces;

namespace Device.Application.Behaviors;

public sealed class DeviceBehaviors(IDeviceRepository repo,IAdapterFactory adapterFactory) : IDevice
{
      public async Task<DeviceDto> CreateAsync(CreateDeviceDto dto)
      {
            var device = new Device.Domain.Entities.Devices(0, dto.ComponentId, dto.Name, dto.SerialNumber, dto.Mac, dto.Ip, dto.Port, dto.Fw, dto.Type, dto.Status, dto.SyncedAt, dto.LocationId,dto.Metadata);
            var res = await repo.CreateAsync(device);
            
            // TODO: Map domain to dto using AutoMapper or similar library
            await adapterFactory.GetAdapter(Venders.AERO).Device.CreateDeviceCommandAsync(dto);

            return new DeviceDto(
                  res.Id,
                  res.ComponentId,
                  res.Name,
                  res.SerialNumber,
                  res.Mac,
                  res.Ip,
                  res.Port,
                  res.Fw,
                  res.Type,
                  res.Status,
                  res.SyncedAt,
                  res.LocationId,
                  res.Metadata
                  );
      }

      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            var adapter = adapterFactory.GetAdapter(Venders.AERO);
            return await adapter.Device.GetIdReportsAsync();
      }
}
