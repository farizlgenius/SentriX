using Core.Application.Interfaces;

namespace Core.Application.ValueObjects;

public sealed class LicenseSetting : ILicenseSetting
{
  public string Secret { get; set; } = string.Empty;

  public string Uri { get; set; } = string.Empty;

  public ILicenseEndpointSetting Endpoint { get; set; } = default!;
}