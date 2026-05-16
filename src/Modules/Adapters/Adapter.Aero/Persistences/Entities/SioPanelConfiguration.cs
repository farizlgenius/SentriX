using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class SioPanelConfiguration : BaseEntity
{

      public short scp_id { get; set; }
      public string mac {get; set;} = string.Empty;
      public short sio_number { get; set; }
      public short n_inputs { get; set; }
      public short n_outputs { get; set; }
      public short n_readers { get; set; }
      public short model { get; set; }
      public short enable { get; set; }
      public short port { get; set; }
      public short address { get; set; }
      public short emax { get; set; }
      public short flags { get; set; }
      public short n_sio_next_in { get; set; }
      public short n_sio_next_out { get; set; }
      public short n_sio_next_rdr { get; set; }
      public int module_id {get; set;}

      public SioPanelConfiguration(){}

            public SioPanelConfiguration(short scp_id, string mac, short sio_number, short n_inputs, short n_outputs, short n_readers, short model, short enable, short port, short address, short emax, short flags, short n_sio_next_in, short n_sio_next_out, short n_sio_next_rdr,int module_id)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.sio_number = sio_number;
            this.n_inputs = n_inputs;
            this.n_outputs = n_outputs;
            this.n_readers = n_readers;
            this.model = model;
            this.enable = enable;
            this.port = port;
            this.address = address;
            this.emax = emax;
            this.flags = flags;
            this.n_sio_next_in = n_sio_next_in;
            this.n_sio_next_out = n_sio_next_out;
            this.n_sio_next_rdr = n_sio_next_rdr;
            this.module_id = module_id;
      }

}
