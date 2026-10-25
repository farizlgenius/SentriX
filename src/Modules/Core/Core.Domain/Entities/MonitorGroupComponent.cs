using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class MonitorGroupComponent
{
      public MonitorType Type {get; private set; }
      public int ComponentId {get; private set; }
      public MonitorGroupComponent(
            MonitorType type,
            int componentId
      )
      {
            Type = type;
            ComponentId = componentId;            
      }
}