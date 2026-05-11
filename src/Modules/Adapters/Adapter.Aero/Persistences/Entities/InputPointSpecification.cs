using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class InputPointSpecification : BaseEntity
{
       public short scp_id { get;  set; }
       public string mac {get; set;} = string.Empty;
      public short sio_number {get;  set;}
      public short input_number { get;  set; }
      public short icvt_num { get;  set; }
      public short debounce { get;  set; }
      public short hold_time { get;  set; }

      public InputPointSpecification(){}

      public InputPointSpecification(short scp_id,string mac, short sio_number, short input_number, short icvt_num, short debounce, short hold_time)
      {
            this.scp_id = scp_id;
            this.mac = mac;
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
