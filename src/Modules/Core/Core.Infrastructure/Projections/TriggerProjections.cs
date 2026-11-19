using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Action;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.Procedure;
using Core.Contract.DTOs.Trigger;

namespace Core.Infrastructure.Projections;

public static class TriggerProjections
{
    // Reusable projection expression for EF Core queries
    private static BaseDto SwitchSrc(Persistences.Entities.Trigger x)
    {
        return x.source_component_type switch
        {
            SharedKernel.Enums.EntityType.DEVICE_MODULE => x.src_device_module == null ? new BaseDto() : new BaseDto(x.src_device_module.guid,x.src_device_module.name),
            SharedKernel.Enums.EntityType.INPUT => x.src_input == null ? new BaseDto() : new BaseDto(x.src_input.guid,x.src_input.name),
            SharedKernel.Enums.EntityType.OUTPUT => x.src_output == null ? new BaseDto() : new BaseDto(x.src_output.guid,x.src_output.name),
            SharedKernel.Enums.EntityType.DOOR => x.src_door == null ? new BaseDto() : new BaseDto(x.src_door.guid,x.src_door.name),
            SharedKernel.Enums.EntityType.TIMEZONE => x.src_timezone == null ? new BaseDto() : new BaseDto(x.src_timezone.guid,x.src_timezone.name),
            SharedKernel.Enums.EntityType.MONITOR_GROUP => x.src_monitor_group == null ? new BaseDto() : new BaseDto(x.src_monitor_group.guid,x.src_monitor_group.name),
            SharedKernel.Enums.EntityType.AREA => x.src_area == null ? new BaseDto() : new BaseDto(x.src_area.guid,x.src_area.name),
            _ => new BaseDto()
        };
    }

    private static Guid SwitchSrcGuid(Persistences.Entities.Trigger x)
    {
        return x.source_component_type switch
        {
            SharedKernel.Enums.EntityType.DEVICE_MODULE => x.src_device_module == null ? Guid.Empty : x.src_device_module.guid,
            SharedKernel.Enums.EntityType.INPUT => x.src_input == null ? Guid.Empty : x.src_input.guid,
            SharedKernel.Enums.EntityType.OUTPUT => x.src_output == null ? Guid.Empty : x.src_output.guid,
            SharedKernel.Enums.EntityType.DOOR => x.src_door == null ? Guid.Empty : x.src_door.guid,
            SharedKernel.Enums.EntityType.TIMEZONE => x.src_timezone == null ? Guid.Empty : x.src_timezone.guid,
            SharedKernel.Enums.EntityType.MONITOR_GROUP => x.src_monitor_group == null ? Guid.Empty : x.src_monitor_group.guid,
            SharedKernel.Enums.EntityType.AREA => x.src_area == null ? Guid.Empty : x.src_area.guid,
            _ => Guid.Empty
        };
    }

    private static string SwitchSrcName(Persistences.Entities.Trigger x)
    {
        return x.source_component_type switch
        {
            SharedKernel.Enums.EntityType.DEVICE_MODULE => x.src_device_module == null ? string.Empty : x.src_device_module.name,
            SharedKernel.Enums.EntityType.INPUT => x.src_input == null ? string.Empty : x.src_input.name,
            SharedKernel.Enums.EntityType.OUTPUT => x.src_output == null ? string.Empty : x.src_output.name,
            SharedKernel.Enums.EntityType.DOOR => x.src_door == null ? string.Empty : x.src_door.name,
            SharedKernel.Enums.EntityType.TIMEZONE => x.src_timezone == null ? string.Empty : x.src_timezone.name,
            SharedKernel.Enums.EntityType.MONITOR_GROUP => x.src_monitor_group == null ? string.Empty : x.src_monitor_group.name,
            SharedKernel.Enums.EntityType.AREA => x.src_area == null ? string.Empty : x.src_area.name,
            _ => string.Empty
        };
    }
    public static readonly Expression<Func<Persistences.Entities.Trigger, TriggerDto>> ToDto = 
        x => new TriggerDto(
            x.guid,
            x.name,
            x.command,
            x.device.guid,
            x.device.name,
            x.trigger_source.guid,
            x.trigger_source.name,
            x.trigger_type.guid,
            x.trigger_type.name,
            x.trigger_map_trigger_codes.Select(x => x.trigger_code.guid),
            x.trigger_arg_map_trigger.Select(x => x.trigger_arg.guid),
            x.trigger_variable_map_trigger.Select(x => x.trigger_variable.guid),
            x.timezone.guid,
            x.timezone.name,
            x.procedure.guid,
            x.procedure.name,
            SwitchSrcGuid(x),
            SwitchSrcName(x),
            x.is_active,
            x.is_default
            );

    public static readonly Expression<Func<Persistences.Entities.Trigger,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        x.device.id,
        x.device.external_id ?? -1,
        x.device.mac,
        x.device.ip,
        -1,
        -1,
        x.device.vendor,
        x.location_id,
        x.id,
        x.external_id
    );
}