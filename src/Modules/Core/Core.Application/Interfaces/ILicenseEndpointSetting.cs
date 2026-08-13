namespace Core.Application.Interfaces;

public interface ILicenseEndpointSetting
{
  public string Exchange { get; }
  public string Demo { get; }
  public string License { get; }
}