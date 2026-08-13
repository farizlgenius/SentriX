namespace Core.Application.Interfaces;

public interface ILicenseSetting
{
  public string Secret { get; }
  public string Uri { get; }
  public ILicenseEndpointSetting Endpoint { get; }

}