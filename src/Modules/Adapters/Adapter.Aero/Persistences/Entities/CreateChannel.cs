using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class CreateChannel : BaseEntity
{
    public short n_channel_id {get; set;}
    public short c_type {get; set;}
    public short c_port {get; set;}
    public short baudrate {get; set;}
    public short timer_1 {get; set;} 
    public short timer_2 {get; set;}
    public short c_model_id {get; set;}
    public short c_rts_mode {get; set;}

    public CreateChannel(){}

      public CreateChannel(short n_channel_id, short c_type, short c_port, short baudrate, short timer_1, short timer_2, short c_model_id, short c_rts_mode)
      {
            this.n_channel_id = n_channel_id;
            this.c_type = c_type;
            this.c_port = c_port;
            this.baudrate = baudrate;
            this.timer_1 = timer_1;
            this.timer_2 = timer_2;
            this.c_model_id = c_model_id;
            this.c_rts_mode = c_rts_mode;
      }
}
