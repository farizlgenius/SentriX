using SharedKernel.Domain;

namespace Input.Domain.Entities;

public sealed class Inputs : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public string Mac {get; private set;} = string.Empty;
      public short DeviceComponentId {get; private set;}
      public short ModuleComponentId { get; private set; }
      public short InputNo {get; private set;}
      public string Metadata {get; private set;} = string.Empty;
      public string Type {get; private set;}  = string.Empty;
      public Inputs(int id,short componentId,
       string name,
       string mac,
       short deviceComponentId,
       short moduleComponentId,
       string metadata,
       int locationId, bool IsActive) : base(id, componentId, locationId, IsActive)
      {
            Name = name;
            Mac = mac;
            DeviceComponentId = deviceComponentId; 
            ModuleComponentId = moduleComponentId;
            Metadata = metadata;

      }
}