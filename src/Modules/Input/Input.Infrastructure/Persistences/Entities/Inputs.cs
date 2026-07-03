using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences.Entities;

public sealed class Inputs : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string mac {get; set;} = string.Empty;
      public short device_component_id {get; set;}
      public short module_component_id {get; set;}
      public short input_no {get; set;}
      public short sensor_mode {get; set;}
      public short debounce {get; set;}
      public short hold_time {get; set;}
      public short log_function {get; set;}
      public short latch_mode {get ;set;}
      public short delay_entry {get;set;}
      public short delay_exit {get ;set;}
      public string type {get; set;}  = string.Empty;

      public Inputs()
      {
      }

      public Inputs(Domain.Entities.Inputs domain) : base(domain.ComponentId, domain.LocationId, domain.IsActive,false)
      {
            this.name = domain.Name;
            this.mac = domain.Mac;
            this.device_component_id = domain.DeviceComponentId;
            this.module_component_id = domain.ModuleComponentId;
            this.input_no = domain.InputNo;
            this.sensor_mode = domain.SensorMode;
            this.debounce = domain.Debounce;
            this.hold_time = domain.HoldTime;
            this.log_function = domain.LogFunction;
            this.latch_mode = domain.LatchMode;
            this.delay_entry = domain.DelayEntry;
            this.delay_exit = domain.DelayExit;
            this.type = domain.Type;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
      }

      public void Update(Domain.Entities.Inputs domain)
      {
            this.name = domain.Name;
            this.input_no = domain.InputNo;
            this.sensor_mode = domain.SensorMode;
            this.debounce = domain.Debounce;
            this.hold_time = domain.HoldTime;
            this.log_function = domain.LogFunction;
            this.latch_mode = domain.LatchMode;
            this.delay_entry = domain.DelayEntry;
            this.delay_exit = domain.DelayExit;
            this.updated_at = DateTime.UtcNow;
      }


}