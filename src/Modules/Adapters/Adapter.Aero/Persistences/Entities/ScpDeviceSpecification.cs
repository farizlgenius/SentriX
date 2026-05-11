using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class ScpDeviceSpecification : BaseEntity
{
       public short scp_id {get; set;}
       public string mac {get; set;} = string.Empty;
      public short n_msp1_port {get; set;} 
      public int n_transcations {get; set;} 
      public short n_sio {get; set;} 
      public short n_mp {get; set;}
      public short n_cp {get; set;}
      public short n_acr {get; set;}
      public short n_alvl {get; set;}
      public short n_trgr {get; set;}
      public short n_proc {get; set;}
      public short gmt_offset {get; set;}
      public short n_dst_id {get; set;}
      public short n_tz {get; set;}
      public short n_hol {get; set;}
      public short n_mpg {get; set;}
      public int n_tran_limit {get; set;}
      public short n_oper_mode {get; set;}
      public short oper_type {get; set;} = 1;
      public short n_language {get; set;} = 0;

      public ScpDeviceSpecification(){}

      public ScpDeviceSpecification(short scp_id, string mac, short n_msp1_port, int n_transcations, short n_sio, short n_mp, short n_cp, short n_acr, short n_alvl, short n_trgr, short n_proc, short gmt_offset, short n_dst_id, short n_tz, short n_hol, short n_mpg, int n_tran_limit, short n_oper_mode, short oper_type, short n_language)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.n_msp1_port = n_msp1_port;
            this.n_transcations = n_transcations;
            this.n_sio = n_sio;
            this.n_mp = n_mp;
            this.n_cp = n_cp;
            this.n_acr = n_acr;
            this.n_alvl = n_alvl;
            this.n_trgr = n_trgr;
            this.n_proc = n_proc;
            this.gmt_offset = gmt_offset;
            this.n_dst_id = n_dst_id;
            this.n_tz = n_tz;
            this.n_hol = n_hol;
            this.n_mpg = n_mpg;
            this.n_tran_limit = n_tran_limit;
            this.n_oper_mode = n_oper_mode;
            this.oper_type = oper_type;
            this.n_language = n_language;
      }
}
