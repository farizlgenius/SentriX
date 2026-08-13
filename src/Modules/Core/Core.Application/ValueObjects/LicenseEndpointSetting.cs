using Core.Application.Interfaces;

namespace Core.Application.ValueObjects;

public sealed class LicenseEndpointSetting : ILicenseEndpointSetting
{
  public string Exchange { get; set; } = string.Empty;

  public string Demo { get; set; } = string.Empty;

  public string License { get; set; } = string.Empty;
}