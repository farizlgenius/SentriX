namespace Aero.Application.Interfaces;

public interface IScpDeviceSpecification
{
  public int nMsp1Port { get; }
  public int nTransaction { get; }
  public int nSio { get; }
  public int nMp { get; }
  public int nCp { get; }
  public int nAcr { get; }
  public int nAlvl { get; }
  public int nTrgr { get; }
  public int nProc { get; }
  public int nDstID { get; }
  public int nTz { get; }
  public int nHol { get; }
  public int nMpg { get; }
  public int nTranLimit { get; }
  public int nOperModes { get; }
  public int OperType { get; }
  public int nLanguages { get; }

}