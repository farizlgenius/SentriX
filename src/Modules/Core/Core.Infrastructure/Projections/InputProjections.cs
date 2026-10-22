using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Input;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class InputProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Input, InputDto>> ToDto = 
        x => new InputDto(
            x.guid,
            x.name,
            x.slot_no,
            x.metadata,
            x.vendor,
            x.device_module.device.guid,
            x.device_module.device.name ?? string.Empty,
            x.device_module.guid,
            x.device_module.name ?? string.Empty,
            x.location == null ? Guid.Empty : x.location.guid,
            x.location == null ? string.Empty : x.location.name ?? string.Empty,
            x.is_active,
            x.is_default
        );

        public static readonly Expression<Func<Persistences.Entities.Input,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        x.device_module.device.id,
        x.device_module.device.external_id ?? -1,
        x.device_module.device.mac,
        x.device_module.device.ip,
        x.device_module.id,
        x.device_module.external_id ?? -1,
        x.vendor,
        x.location_id,
        x.id,
        x.external_id
    );
}