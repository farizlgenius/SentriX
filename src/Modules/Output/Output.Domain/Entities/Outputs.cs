using System.Xml.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SharedKernel.Helpers;

namespace Output.Domain.Entities;

public sealed class Outputs
{
      public string Name { get; private set; } = string.Empty;
      public int ModuleId { get; private set; }
      public short OutputNo {get; private set;}
      public string Model {get; private set;} = string.Empty;
      public int LocationId {get; private set;}
      public short DefaultPulse {get; private set;}

        public Outputs(string name, int moduleId,short outputNo,string model,int location,short pulse)
      {
            ValidationHelper.ValidateNotNullOrEmpty(name,nameof(Name));
            ValidationHelper.ValidateNotMinus(moduleId,nameof(ModuleId));
            ValidationHelper.ValidateNotMinus(location,nameof(LocationId));
            Name = name;
            ModuleId = moduleId;
            OutputNo = outputNo;
            Model =  model;
            LocationId = location;
            DefaultPulse = pulse;
      }

}