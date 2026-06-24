using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences.Entities;

public sealed class OutputDriveMode : BaseEntity
{
          public string label {get; set;} = string.Empty;
      public short value {get; set;} 
      public string description {get; set;} = string.Empty;

      public OutputDriveMode(){}
      public OutputDriveMode(string label,short value,string description) : base(0,0,true)
      {
            this.label = label;
            this.value = value;
            this.description = description;
      }



}

