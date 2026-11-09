using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Trigger : BaseEntity
{
      public TriggerCommand command { get; set; }
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
      public int? src_monitor_group {get; set; }
      public MonitorGroup? src_monitor_group_id {get; set; }
      public int? src_area {get; set; }
      public Area? src_area_id {get; set; }

      // Relation
      public int device_id {get; set; }
      public Device device {get; set; } = default!;
      public int? procedure_id {get; set; }
      public Procedure? procedure {get; set; } 
      public int trigger_source_id {get; set; }
      public TriggerSource trigger_source {get; set; } = default!;

      public int trigger_type_id {get; set; }
      public TriggerType trigger_type { get; set; } = default!;
      public ICollection<TriggerCode> trigger_codes {get; set; } = default!;
      public int timezone_id {get; set; }
      public TimeZone timezone {get; set; } = default!;
      public Trigger(){}
      public Trigger() : base()
      {
            
      }

}