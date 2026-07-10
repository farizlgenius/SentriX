namespace Adapter.Amico.Persistences.Entities;

public sealed class Amicos : BaseDbEntity
{
      public string mac { get; set; } = string.Empty;
      public string ip { get; set; } = string.Empty;
      public string session { get; set; } = string.Empty;
      public Amicos()
      {
            
      }

      public Amicos(Guid guid,string mac,string ip,string session ,int locationId, bool isactive, bool isdefault) : base(guid, locationId, isactive, isdefault)
      {
            this.mac = mac;
            this.ip = ip;
            this.session = session;
      }

      public void UpdateSession(string session)
      {
            this.session = session;
            this.updated_at = DateTime.UtcNow;
      }
}