using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences.Entities;

public sealed class Outputs : BaseEntity
{
      public short component_id { get; set; }
      public string name { get; set; } = string.Empty;
      public int module_id { get; set; }
      public short output_no { get; set; }
      public short relay_mode { get; set; }
      public short offline_mode { get; set; }
      public short default_pulse { get; set; } = 1;

      public Outputs() { }


    
}

