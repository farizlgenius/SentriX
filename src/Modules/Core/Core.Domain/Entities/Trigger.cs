using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Trigger : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public TriggerCommand Command {get; private set; } = TriggerCommand.EXE_ACTION_PREFIX_0;
      public EntityType SourceComponentType {get; private set; } = EntityType.DEVICE;
      public int DeviceId {get; private set; }
      public int ProcedureId {get; private set; }
      public int TriggerSourceId {get; private set; }
      public int TriggerTypeId {get; private set; }
      public IEnumerable<int> TriggerCodeIds {get; private set; }
      public IEnumerable<int> TriggerArgIds {get; private set; }
      public int? FirstTriggerVariavleId {get; private set;}
      public int? SecondTriggerVariavleId {get; private set;}
      public int? ThirdTriggerVariavleId {get; private set;}
      public int? ForthTriggerVariavleId {get; private set;}
      public int TimeZoneId {get; private set; }
      public int? SrcDeviceModuleId {get; private set; }
      public int? SrcInputId {get; private set; }
      public int? SrcOutputId {get; private set; }
      public int? SrcDoorId {get; private set; }
      public int? SrcTimeZoneId {get; private set; }
      public int? SrcMonitorGroupId {get; private set; }
      public int? SrcAreaId {get; private set; }
      public Trigger(
            string name,
            int deviceId,
            int procedureId,
            TriggerCommand command,
            int triggerSourceId,
            int triggerTypeId,
            IEnumerable<int> triggerCodeIds,
            IEnumerable<int> triggerArgIds,
            int timezoneId,
            short externalId,
            int? firstTriggerVariableId=null,
            int? secondTriggerVariableId=null,
            int? thirdTriggerVariableId=null,
            int? forthTriggerVariableId=null,
            int? srcDeviceModuleId=null,
            int? srcInputId=null,
            int? srcOutputId=null,
            int? srcDoorId=null,
            int? srcTimeZoneId=null,
            int? srcMonitorGroupId=null,
            int? srcAreaId=null
            ) : base(Guid.NewGuid(),externalId)
      {
            Name = name;
            DeviceId = deviceId;
            ProcedureId = procedureId;
            TriggerSourceId = triggerSourceId;
            Command = command;
            TriggerTypeId = triggerTypeId;
            TriggerCodeIds = triggerCodeIds;
            TriggerArgIds = triggerArgIds;
            SrcDeviceModuleId = srcDeviceModuleId;
            SrcInputId = srcInputId;
            SrcOutputId = srcOutputId;
            SrcDoorId = srcDoorId;
            SrcTimeZoneId = srcTimeZoneId;
            SrcMonitorGroupId = srcMonitorGroupId;
            SrcAreaId = srcAreaId;
            TimeZoneId = timezoneId;
            FirstTriggerVariavleId = firstTriggerVariableId;
            SecondTriggerVariavleId = secondTriggerVariableId;
            ThirdTriggerVariavleId = thirdTriggerVariableId;
            ForthTriggerVariavleId = forthTriggerVariableId;
            
      }

      public Trigger(
            Guid guid,
            string name,
            int deviceId,
            int procedureId,
            TriggerCommand command,
            int triggerSourceId,
            int triggerTypeId,
            IEnumerable<int> triggerCodeIds,
            IEnumerable<int> triggerArgeIds,
            short externalId,
            int? firstTriggerVariableId=null,
            int? secondTriggerVariableId=null,
            int? thirdTriggerVariableId=null,
            int? forthTriggerVariableId=null,
            int? srcDeviceModuleId=null,
            int? srcInputId=null,
            int? srcOutputId=null,
            int? srcDoorId=null,
            int? srcTimeZoneId=null,
            int? srcMonitorGroupId=null,
            int? srcAreaId=null
            ) : base(guid,externalId)
      {
            Name = name;
            DeviceId = deviceId;
            Command = command;
            ProcedureId = procedureId;
            TriggerSourceId = triggerSourceId;
            TriggerTypeId = triggerTypeId;
            TriggerCodeIds = triggerCodeIds;
            TriggerArgIds = triggerArgeIds;
            SrcDeviceModuleId = srcDeviceModuleId;
            SrcInputId = srcInputId;
            SrcOutputId = srcOutputId;
            SrcDoorId = srcDoorId;
            SrcTimeZoneId = srcTimeZoneId;
            SrcMonitorGroupId = srcMonitorGroupId;
            SrcAreaId = srcAreaId;
            FirstTriggerVariavleId = firstTriggerVariableId;
            SecondTriggerVariavleId = secondTriggerVariableId;
            ThirdTriggerVariavleId = thirdTriggerVariableId;
            ForthTriggerVariavleId = forthTriggerVariableId;
      }
}