using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class AdapterEvent : BaseDomain
{
  public string Name { get; private set; }
  public string Mac { get; private set; } = string.Empty;
  public int ComponentId { get; private set; }
  public string Command { get; private set; } = string.Empty;
  public int Tag { get; private set; }
  public DateTime SendAt { get; private set; }
  public DateTime? ReceivedAt { get; private set; }
  public string Body { get; private set; } = string.Empty;
  public CommandStatus Status { get; private set; } = CommandStatus.PENDING;
  public string Reason { get; private set; } = string.Empty;
  public string Response { get; private set; } = string.Empty;
  public SharedKernel.Enums.Vendor Vendor { get; private set; } = SharedKernel.Enums.Vendor.aero;
  public int LocationId { get; private set; }

  public AdapterEvent(
    Guid guid,
        string name,
        string mac,
        short component_id,
        string command,
        int tag,
        DateTime send_at,
        DateTime? received_at,
        string body,
        CommandStatus status,
        string reason,
        string response,
        SharedKernel.Enums.Vendor vendor,
        int locationId
  ) : base(guid)
  {
    Name = name;
    Mac = mac;
    ComponentId = component_id;
    Command = command;
    Tag = tag;
    SendAt = send_at;
    ReceivedAt = received_at;
    Body = body;
    Status = status;
    Reason = reason;
    Response = response;
    Vendor = vendor;
    LocationId = locationId;
  }

  public AdapterEvent(

        string name,
        string mac,
        short component_id,
        string command,
        int tag,
        DateTime send_at,
        DateTime? received_at,
        string body,
        CommandStatus status,
        string reason,
        string response,
        SharedKernel.Enums.Vendor vendor,
        int locationId
  ) : base(Guid.NewGuid())
  {
    Name = name;
    Mac = mac;
    ComponentId = component_id;
    Command = command;
    Tag = tag;
    SendAt = send_at;
    ReceivedAt = received_at;
    Body = body;
    Status = status;
    Reason = reason;
    Response = response;
    Vendor = vendor;
    LocationId = locationId;
  }

  
}



