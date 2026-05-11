using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class SystemLevelSpecification : BaseEntity
{
     public short n_ports {get;  set;} = 1024;
    public short n_scps {get;  set;} = 1024;
    public short n_timezones {get;  set;} = 0;
    public short n_holidays {get;  set;} = 0;
    public short b_direct_mode {get;  set;} = 1;
    public short debug_rq {get;  set;} = 0;
    public short n_debug_arg {get;  set;} = 0;

    public SystemLevelSpecification(){}

      public SystemLevelSpecification(short n_ports, short n_scps, short n_timezones, short n_holidays, short b_direct_mode, short debug_rq, short n_debug_arg)
      {
            this.n_ports = n_ports;
            this.n_scps = n_scps;
            this.n_timezones = n_timezones;
            this.n_holidays = n_holidays;
            this.b_direct_mode = b_direct_mode;
            this.debug_rq = debug_rq;
            this.n_debug_arg = n_debug_arg;
      }
}
