using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Device : BaseDomain
{
  public string Name { get; private set; } = string.Empty;
  public string SerialNumber { get; private set; } = string.Empty;
  public string Mac { get; private set; } = string.Empty;
  public string Ip { get; private set; } = string.Empty;
  public int Port { get; set; }
  public string Fw { get; set; } = string.Empty;
  public string Status { get; private set; } = string.Empty;
  public DateTime SyncedAt { get; private set; } = default!;
  public string Metadata { get; private set; } = string.Empty;
  public Guid LocationGuid { get; private set; } = default!;


  public Device(
   string Name,
   string SerialNumber,
   string Mac,
   string Ip,
   int Port,
   string Fw,
   string Status,
   DateTime SyncedAt,
   string Metadata,
   Guid LocationGuid
   )
  {
    ValidationHelper.IsValidName(Name);
    ValidationHelper.ValidateGuid(LocationGuid,nameof(LocationGuid));
    ValidationHelper.IsNullOrEmpty(SerialNumber,nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac,nameof(Mac));
    ValidationHelper.IsNullOrEmpty(Ip,nameof(Ip));
    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Mac = Mac;
    this.Ip = Ip;
    this.Port = Port;
    this.Fw = Fw;
    this.Status = Status;
    this.SyncedAt = SyncedAt;
    this.Metadata = Metadata;
    this.LocationGuid = LocationGuid;
  }

  public Device(
    Guid Guid,
    string Name,
    string SerialNumber,
    string Mac,
    string Ip,
    int Port,
    string Fw,
    string Status,
    DateTime SyncedAt,
    string Metadata,
    Guid LocationGuid
    ) : base(Guid)
  {
    ValidationHelper.IsValidName(Name);
    ValidationHelper.ValidateGuid(LocationGuid,nameof(LocationGuid));
    ValidationHelper.IsNullOrEmpty(SerialNumber,nameof(SerialNumber));
    ValidationHelper.IsNullOrEmpty(Mac,nameof(Mac));
    ValidationHelper.IsNullOrEmpty(Ip,nameof(Ip));
    this.Name = Name;
    this.SerialNumber = SerialNumber;
    this.Mac = Mac;
    this.Ip = Ip;
    this.Port = Port;
    this.Fw = Fw;
    this.Status = Status;
    this.SyncedAt = SyncedAt;
    this.Metadata = Metadata;
    this.LocationGuid = LocationGuid;
  }
}