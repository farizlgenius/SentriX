using SharedKernel.Domain;

namespace Input.Domain.Entities;

public sealed class InputList : BaseDomain
{
      public short Type {get; private set;}
      public short Number {get; private set;}
      public InputList(int id,short type, short number, int locationId, bool IsActive) : base(id, 0, locationId, IsActive)
      {
            this.Type = type;
            this.Number = number;
      }
}