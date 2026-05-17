using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class OutputPointSpecification : BaseEntity
{
      public int aero_id {get; set;}
      public Aeros aero {get; set;} = default!;
      public int sio_number { get; set; }
      public short output { get; set; }
      public short mode { get; set; }

      public OutputPointSpecification(){}

      public OutputPointSpecification(int aero_id, int sio_number, short output, short mode)
      {
            this.aero_id = aero_id;
            this.sio_number = sio_number;
            this.output = output;
            this.mode = mode;
      }
}