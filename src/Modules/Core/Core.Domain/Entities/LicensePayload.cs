namespace Core.Domain.Entities;

public sealed class LicensePayload : BaseDomain
{
  public int Version { get; set; }
  public Guid LicenseGuid { get; set; }
  public string BackendId { get; set; } = null!;
  public string Customer { get; set; } = null!;
  public string EndUser { get; set; } = null!;
  public string Product { get; set; } = null!;
  public string Edition { get; set; } = null!;
  public DateTime IssuedAtUtc { get; set; }
  public DateTime ExpiresAtUtc { get; set; }
  public SentrixLimit? Limits { get; set; }
  public string MachineId { get; set; } = null!;

  public LicensePayload(int version, Guid licenseGuid, string backendId, string customer, string endUser, string product, string edition, DateTime issuedAtUtc, DateTime expiresAtUtc, SentrixLimit? limits, string machineBinding)
  {
    Version = version;
    LicenseGuid = licenseGuid;
    BackendId = backendId;
    Customer = customer;
    EndUser = endUser;
    Product = product;
    Edition = edition;
    IssuedAtUtc = issuedAtUtc;
    ExpiresAtUtc = expiresAtUtc;
    Limits = limits;
    MachineId = machineBinding;
  }


}