using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class InputPointSpecification : BaseEntity
{
      public int aero_id {get; set;}
      public Aeros aero {get; set;} = default!;
      public short sio_number {get;  set;}
      public short input_number { get;  set; }
      public short icvt_num { get;  set; }
      public short debounce { get;  set; }
      public short hold_time { get;  set; }

      public InputPointSpecification(){}

      public InputPointSpecification(int aero_id, short sio_number, short input_number, short icvt_num, short debounce, short hold_time)
      {
            this.aero_id = aero_id;
            this.sio_number = sio_number;
            this.input_number = input_number;
            this.icvt_num = icvt_num;
            this.debounce = debounce;
            this.hold_time = hold_time;
      }

      public void UpdateInputNumber(short input_number)
      {
            this.input_number = input_number;
            this.updated_at = DateTime.UtcNow;
      }



}
