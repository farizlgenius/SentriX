using System;
using SharedKernel.Domain;

namespace Operator.Infrastructure.Persistences.Entities;

public sealed class OperatorLocation : BaseEntity
{
      public int operator_id { get; set; }
      public Operators operators { get; set; } = null!;
      public int location_id { get; set; }
      public OperatorLocation() { }

}
