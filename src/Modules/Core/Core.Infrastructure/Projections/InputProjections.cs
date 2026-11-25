using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Monitor;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class InputProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Monitor, MonitorDto>> ToDto = 
        x => new MonitorDto(
            x.guid,
            x.name,
            x.slot_no,
            x.metadata,
            new BaseDto(
               x.device_module.device.guid,
               x.device_module.device.name,
               x.device_module.device.vendor
            ),
            new BaseDto(
                x.device_module.guid,
                x.device_module.name,
                x.device_module.device.vendor
            ),
            x.is_active,
            x.is_default
        );

        public static readonly Expression<Func<Persistences.Entities.Monitor,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        x.device_module.device.id,
        x.device_module.device.external_id ?? -1,
        x.device_module.device.mac,
        x.device_module.device.ip,
        x.device_module.id,
        x.device_module.external_id ?? -1,
        x.device_module.device.vendor,
        x.location_id,
        x.id,
        x.external_id
    );
}