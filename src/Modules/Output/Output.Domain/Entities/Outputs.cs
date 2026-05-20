using System.Xml.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Output.Domain.Entities;

public sealed class Outputs : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Mac {get; private set;} = string.Empty;
      public short DeviceComponentId {get; private set;}
      public short ModuleComponentId { get; private set; }
      public short OutputNo {get; private set;}
      public string Model {get; private set;} = string.Empty;
      public short Mode {get; private set;}
      public short DefaultPulse {get; private set;}
      public string Type {get; private set;}  = string.Empty;

        public Outputs(int id,short componentId,string mac,string name,short deviceComponentId, short moduleComponentId,short outputNo,string model,short mode,string type,int location,short pulse,bool isActive) : base(id,componentId,location,isActive)
      {
            ValidationHelper.ValidateNotNullOrEmpty(name,nameof(Name));
            ValidationHelper.ValidateNotNullOrEmpty(mac,nameof(Mac));
            ValidationHelper.ValidateNotMinus(deviceComponentId,nameof(DeviceComponentId));
            ValidationHelper.ValidateNotMinus(moduleComponentId,nameof(ModuleComponentId));
            ValidationHelper.ValidateNotMinus(location,nameof(LocationId));
            ValidationHelper.ValidateNotMinus(mode,nameof(Mode));
            Name = name;
            Mac = mac;
            DeviceComponentId = deviceComponentId;
            ModuleComponentId = moduleComponentId;
            OutputNo = outputNo;
            Model =  model;
            Mode = mode;
            Type = type;
            DefaultPulse = pulse;
      }

}