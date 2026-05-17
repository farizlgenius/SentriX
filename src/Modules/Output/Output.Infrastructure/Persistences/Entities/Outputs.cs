using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences.Entities;

public sealed class Outputs : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public int module_id { get; set; }
      public string metadata { get; set; } = string.Empty;
      public int location_id { get; set; }


      public Outputs() { }

      public Outputs(Domain.Entities.Outputs domain)
      {
            name = domain.Name;
            module_id = domain.ModuleId;
            metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }



}

