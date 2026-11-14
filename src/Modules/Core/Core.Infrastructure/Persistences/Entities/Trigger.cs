using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Trigger : BaseEntity
{
      public TriggerCommand command { get; set; }
      public EntityType source_component_type {get; set; }
      // Source
      public int? src_device_module_id {get; set; }
      public DeviceModule? src_device_module {get; set; }
      public int? src_input_id {get; set; }
      public Input? src_input {get; set; }
      public int? src_output_id {get; set; }
      public Output? src_output {get; set; }
      public int? src_door_id {get; set; }
      public Door? src_door {get; set; }
      public int? src_timezone_id {get; set; }
      public TimeZone? src_timezone {get; set; }
      public int? src_monitor_group_id {get; set; }
      public MonitorGroup? src_monitor_group {get; set; }
      public int? src_area_id {get; set; }
      public Area? src_area {get; set; }

      // Relation
      public int device_id {get; set; }
      public Device device {get; set; } = default!;
      public int procedure_id {get; set; }
      public Procedure procedure {get; set; } = default!;
      public int trigger_source_id {get; set; }
      public TriggerSource trigger_source {get; set; } = default!;

      public int trigger_type_id {get; set; }
      public TriggerType trigger_type { get; set; } = default!;
      public ICollection<TriggerMapTriggerCode> trigger_map_trigger_codes {get; set; } = default!;
      public int timezone_id {get; set; }
      public TimeZone timezone {get; set; } = default!;
      public ICollection<TriggerVariableMapTrigger> trigger_variable_map_trigger {get; set; } = default!;
      public ICollection<TriggerArgMapTrigger> trigger_arg_map_trigger {get; set; } = default!;

      public Trigger(){}
      public Trigger(Domain.Entities.Trigger d) : base(d.Guid,d.ExternalId,d.AltrExternalId)
      {
            command = d.Command;
            source_component_type = d.SourceComponentType;
            src_device_module_id = d.SrcDeviceModuleId;
            src_input_id = d.SrcInputId;
            src_output_id = d.SrcOutputId;
            src_door_id = d.SrcDoorId;
            src_timezone_id = d.SrcTimeZoneId;
            src_monitor_group_id = d.SrcMonitorGroupId;
            src_area_id = d.SrcAreaId;
            device_id = d.DeviceId;
            procedure_id = d.ProcedureId;
            trigger_source_id = d.TriggerSourceId;
            trigger_type_id  = d.TriggerTypeId;
            timezone_id = d.TimeZoneId;
            trigger_map_trigger_codes = [.. d.TriggerCodeIds.Select(x => new TriggerMapTriggerCode(x))];
            trigger_arg_map_trigger = [.. d.TriggerArgIds.Select(x => new TriggerArgMapTrigger(x))];
            var triggerVar = new List<TriggerVariableMapTrigger>();
            if(d.FirstTriggerVariavleId != null)
            {
                  triggerVar.Add(
                        new TriggerVariableMapTrigger(TriggerVariableIndex.ZERO,d.FirstTriggerVariavleId ?? 0)
                  );
            }

            if(d.SecondTriggerVariavleId != null)
            {
                  triggerVar.Add(
                        new TriggerVariableMapTrigger(TriggerVariableIndex.ONE,d.SecondTriggerVariavleId ?? 0)
                  );
            }

            if(d.ThirdTriggerVariavleId != null)
            {
                  triggerVar.Add(
                        new TriggerVariableMapTrigger(TriggerVariableIndex.TWO,d.ThirdTriggerVariavleId ?? 0)
                  );
            }

            if(d.ForthTriggerVariavleId != null)
            {
                  triggerVar.Add(
                        new TriggerVariableMapTrigger(TriggerVariableIndex.THREE,d.ForthTriggerVariavleId ?? 0)
                  );
            }

            trigger_variable_map_trigger = triggerVar;
      }

}