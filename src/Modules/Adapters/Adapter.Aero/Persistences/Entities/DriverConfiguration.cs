using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class DriverConfiguration : BaseEntity
{
      public short scp_id { get; set; }
      public string mac {get; set;} = string.Empty;
      public short msp1_number { get; set; }
      public short port_number { get; set; }
      public short baudrate { get; set; }
      public short reply_time { get; set; }
      public short n_protocol { get; set; }
      public short n_dialect { get; set; }
      public DriverConfiguration(){}

      public DriverConfiguration(short scp_id,string mac,short msp1_number, short port_number, short baudrate, short reply_time, short n_protocol, short n_dialect)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.msp1_number = msp1_number;
            this.port_number = port_number;
            this.baudrate = baudrate;
            this.reply_time = reply_time;
            this.n_protocol = n_protocol;
            this.n_dialect = n_dialect;
      }
}
