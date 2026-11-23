using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ActionTarget
{
      [Key]
      public int id {get; set; }
      public int? door_id {get; set; }
      public Door? door {get; set; }
      public int? monitor_id {get; set; }
      public Monitor? monitor {get; set; }
      public int? control_id {get; set; }
      public Control? control {get; set; }
      public int? area_id {get; set; }
      public Area? area {get; set; }
      public int? user_id {get; set; }
      public User? user {get; set; }
      public int? trigger_variable_id {get; set; }
      public TriggerVariable? trigger_variable {get; set; }
      public int? procedure_id {get; set; }
      public Procedure? procedure {get; set; } 
      public int? timezone_id {get; set; }
      public TimeZone? timezone {get; set; }
      public int? monitor_group_id {get; set; }
      public MonitorGroup? monitor_group {get; set; }
      public int? trigger_id {get; set; }
      public Trigger? trigger {get; set; }
      // Relation
      public int action_id {get; set; }
      public Action action {get; set; } = default!;

      public ActionTarget(){}

      public ActionTarget(Domain.Entities.ActionTarget d)
      {
            door_id = d.DoorId;
            monitor_id = d.MonitorId;
            control_id = d.ControlId;
            area_id = d.AreaId;
            user_id = d.UserId;
            trigger_variable_id = d.TriggerVariableId;
            timezone_id = d.TimeZoneId;
            monitor_group_id = d.MonitorGroupId;
            trigger_id = d.TriggerId;
      }
}