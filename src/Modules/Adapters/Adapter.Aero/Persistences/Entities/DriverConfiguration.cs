using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class DriverConfiguration : BaseEntity
{
      public short msp1_number { get; set; }
      public short port_number { get; set; }
      public short baudrate { get; set; }
      public short reply_time { get; set; }
      public short n_protocol { get; set; }
      public short n_dialect { get; set; }
      public int aero_id {get; set;}
      public Aeros aero {get;set;} = default!;
      public DriverConfiguration(){}

      public DriverConfiguration(int aero_id,short msp1_number, short port_number, short baudrate, short reply_time, short n_protocol, short n_dialect)
      {
            this.aero_id = aero_id;
            this.msp1_number = msp1_number;
            this.port_number = port_number;
            this.baudrate = baudrate;
            this.reply_time = reply_time;
            this.n_protocol = n_protocol;
            this.n_dialect = n_dialect;
      }
}
