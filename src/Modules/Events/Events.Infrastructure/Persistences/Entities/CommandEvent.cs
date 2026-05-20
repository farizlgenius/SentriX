using SharedKernel.Domain;

namespace Events.Infrastructure.Persistences.Entities;

public sealed class CommandEvent : BaseEntity
{
       public string mac {get; set;} = string.Empty;
      public string command {get; set;} = string.Empty;
      public int tag {get; set;}
      public DateTime send_at {get; set;}
      public DateTime received_at {get; set;}
      public string body {get; set;}  =string.Empty;
      public string status {get; set;} = string.Empty;
      public string reason {get; set;} = string.Empty;

      public CommandEvent(){}

      public CommandEvent(string mac, short component_id, string command, int tag, DateTime send_at, DateTime received_at, string body, string status, string reason)
      {
            this.mac = mac;
            this.component_id = component_id;
            this.command = command;
            this.tag = tag;
            this.send_at = send_at;
            this.received_at = received_at;
            this.body = body;
            this.status = status;
            this.reason = reason;
      }

      public void Update(string status,string reason)
      {
            received_at = DateTime.UtcNow;
            updated_at = DateTime.UtcNow;
            this.status = status;
            this.reason = reason;
      }
}

