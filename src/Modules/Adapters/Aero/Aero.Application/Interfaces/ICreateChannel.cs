namespace Aero.Application.Interfaces;

public interface ICreateChannel
{
  public int nChannelId { get; set; }
  public int cType { get; set; }
  public int cPort { get; set; }
}

