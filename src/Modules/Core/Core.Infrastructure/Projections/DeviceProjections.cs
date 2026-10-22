using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Device;
using Core.Contract.DTOs.DeviceModule;

namespace Core.Infrastructure.Projections;

public static class DeviceProjections
{
      // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Device, DeviceDto>> ToDto = 
        x => new DeviceDto(
                        x.guid,
                        x.name,
                        x.serial_number,
                        x.mac,
                        x.ip,
                        x.port,
                        x.firmware,
                        x.vendor,
                        x.metadata,
                        x.synced_at,
                        x.configuration_status,
                        x.device_modules.Select(d => new DeviceModuleDto(
                              d.guid,
                              d.name,
                              d.serial_number,
                              d.mac,
                              d.firmware,
                              d.port,
                              d.address,
                              d.model,
                              d.reader_slot,
                              d.output_slot,
                              d.input_slot,
                              d.device.guid,
                              d.device.name,
                              d.location.guid,
                              d.location.name,
                              d.is_active,
                              d.is_default
                        )).ToList(),
                        x.location == null ? Guid.Empty : x.location.guid,
                        x.is_active,
                        x.is_default
                  );

    public static readonly Expression<Func<Persistences.Entities.Device,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        x.id,
        x.external_id ?? -1,
        x.mac,
        x.ip,
        -1,
        -1,
        x.vendor,
        x.location_id,
        x.id,
        x.external_id
    );
}