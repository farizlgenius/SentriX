using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences.Entities;

public sealed class Outputs : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public int module_id { get; set; }
      public short output_no {get; set;}
      public string model {get; set;} = string.Empty;
      public short default_pulse { get; set; } 
      public int location_id { get; set; }


      public Outputs() { }

      public Outputs(Domain.Entities.Outputs domain)
      {
            name = domain.Name;
            module_id = domain.ModuleId;
            output_no = domain.OutputNo;
            model = domain.Model;
            default_pulse = domain.DefaultPulse;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }



}

