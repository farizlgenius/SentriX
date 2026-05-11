using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed  class WriterAudit : BaseEntity
{
      public string mac {get; set;} = string.Empty;
      public int scp_id {get; set;}
      public int command_code {get; set;}
      public string command {get; set;} = string.Empty;
      public int tag {get; set;}
      public DateTime send_at {get; set;}
      public DateTime received_at {get; set;}
      public string body {get; set;}  =string.Empty;
      public string status {get; set;} = string.Empty;
      public bool is_nak {get; set;}
      public string reason {get; set;} = string.Empty;

      public WriterAudit(){}

      public WriterAudit(string mac, int scp_id, int command_code, string command, int tag, DateTime send_at, DateTime received_at, string body, string status, bool is_nak, string reason)
      {
            this.mac = mac;
            this.scp_id = scp_id;
            this.command_code = command_code;
            this.command = command;
            this.tag = tag;
            this.send_at = send_at;
            this.received_at = received_at;
            this.body = body;
            this.status = status;
            this.is_nak = is_nak;
            this.reason = reason;
      }

      public void Update(string status,bool is_nak,string reason)
      {
            received_at = DateTime.UtcNow;
            updated_at = DateTime.UtcNow;
            this.status = status;
            this.is_nak = is_nak;
            this.reason = reason;
      }
}
