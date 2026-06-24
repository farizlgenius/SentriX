using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences.Entities;

public sealed class OutputMode : BaseEntity
{
          public string label {get; set;} = string.Empty;
      public short value {get; set;} 
      public short drive {get; set;}
      public short offline {get; set;}

      public OutputMode(){}


}

