using System.Xml.Schema;
using SharedKernel.Helpers;

namespace Output.Domain.Entities;

public sealed class Outputs
{
      public string Name { get; private set; } = string.Empty;
      public int ModuleId { get; private set; }
      public int LocationId {get; private set;}
      public string Metadata {get; private set;} = string.Empty;

        public Outputs(string name, int moduleId,int location,string metadata)
      {
            ValidationHelper.ValidateNotNullOrEmpty(name,nameof(Name));
            ValidationHelper.ValidateNotMinus(moduleId,nameof(ModuleId));
            ValidationHelper.ValidateNotMinus(location,nameof(LocationId));
            Name = name;
            ModuleId = moduleId;
            LocationId = location;
            Metadata = metadata;
      }

}