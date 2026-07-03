using SharedKernel.Domain;

namespace Input.Domain.Entities;

public sealed class Inputs : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public string Mac {get; private set;} = string.Empty;
      public short DeviceComponentId {get; private set;}
      public short ModuleComponentId { get; private set; }
      public short InputNo {get; private set;}
      public short SensorMode { get; private set; }
      public short Debounce { get; private set; }
      public short HoldTime { get; private set; }
      public short LogFunction { get; private set; }
      public short LatchMode { get; private set; } 
      public short DelayEntry { get; private set; }     
      public short DelayExit { get; private set; }    
      public string Type {get; private set;}  = string.Empty;
      public Inputs(
      int id,
      short componentId,
       string name,
       string mac,
       short deviceComponentId,
       short moduleComponentId,
       short inputNo,
       short sensorMode,
       short debounce,
       short holdTime,
       short logFunction,
       short latchMode,
       short delayEntry,
       short delayExit,
       string type,
       int locationId, bool IsActive) : base(id, componentId, locationId, IsActive)
      {
            Name = name;
            Mac = mac;
            DeviceComponentId = deviceComponentId; 
            ModuleComponentId = moduleComponentId;
            InputNo = inputNo;
            SensorMode = sensorMode;
            Debounce = debounce;
            HoldTime = holdTime;
            LogFunction = logFunction;
            LatchMode = latchMode;
            DelayEntry = delayEntry;
            DelayExit = delayExit;
            Type = type;

      }
}