using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class AuditTrail : BaseDomain
{
      public string Entity { get; set; } = string.Empty;       // e.g., Product

      public AuditAction Action { get; set; } = AuditAction.Read;
      public string Username { get; set; } = string.Empty;
      public string Ip { get; set; } = string.Empty;                    // Helpful for security auditing
      public Guid? ObjectGuid {get; set;}
    public string? ObjectName {get; set;}

      public string? Detail { get; set; }                        // JSON delta/changes only

      public int? LocationId {get; set;}

      public AuditTrail(
            string entity,
            AuditAction action,
            string username,
            string ip,
            Guid? objectGuid,
            string? name,
            string? detail,
            int? locationId
      ) : base(Guid.NewGuid())
      {
            Entity = entity;
            Action = action;
            Username = username;
            Ip = ip;
            ObjectGuid = objectGuid;
            ObjectName = name;
            Detail = detail;
            LocationId = locationId;
      }
}