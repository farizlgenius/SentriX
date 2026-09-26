using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AuditTrail : BaseEntity
{
    public string entity { get; set; } = string.Empty;       // e.g., Product

    public AuditAction action { get; set; } = AuditAction.Read;
    public string username { get; set; } = string.Empty;
    public string ip { get; set; } =  string.Empty;                   // Helpful for security auditing
    public Guid? object_guid {get; set;}
    public string? object_name {get; set;}

    public string? detail { get; set; }                        // JSON delta/changes only

    public int? location_id {get; set;}
    public Location? location {get; set;}

    public AuditTrail() { }
     public AuditTrail(Core.Domain.Entities.AuditTrail d) : base(d.Guid)
    {
        entity = d.Entity;
        action = d.Action;
        username = d.Username;
        ip = d.Ip;
        detail = d.Detail;
        if(d.LocationId != 0)
        {
            location_id = d.LocationId;
        }    
    }
}