using Core.Application.Interfaces;

namespace Core.Application.ValueObjects;

public sealed class LicenseSetting : ILicenseSetting
{
  public string Secret { get; set; } = string.Empty;

  public string Uri { get; set; } = string.Empty;

  // ✅ Concrete type allows ConfigurationBinder to instantiate it
    public LicenseEndpointSetting Endpoint { get; set; } = new();

    // ✅ Explicit interface implementation guarantees ILicenseSetting compatibility
    ILicenseEndpointSetting ILicenseSetting.Endpoint => Endpoint;
}