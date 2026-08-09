using System.ComponentModel.DataAnnotations;

namespace Adapter.Amico.Persistences.Entities;

public sealed class Amicos
{
      [Key]
      public int id { get; set; }
      public Guid guid { get; set; }
      public string mac { get; set; } = string.Empty;
      public string ip { get; set; } = string.Empty;
      public string session { get; set; } = string.Empty;
      public Amicos()
      {

      }

      public Amicos(Guid guid, string mac, string ip, string session)
      {
            this.mac = mac;
            this.ip = ip;
            this.session = session;
      }

      public void UpdateSession(string session)
      {
            this.session = session;
            // this.updated_at = DateTime.UtcNow;
      }
}