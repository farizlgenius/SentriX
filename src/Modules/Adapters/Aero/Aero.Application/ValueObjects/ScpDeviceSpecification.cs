using Aero.Application.Interfaces;

namespace Aero.Application.ValueObjects;

public sealed class ScpDeviceSpecification : IScpDeviceSpecification
{
  public int nMsp1Port { get; set; }
  public int nTransaction { get; set; }
  public int nSio { get; set; }
  public int nMp { get; set; }
  public int nCp { get; set; }
  public int nAcr { get; set; }
  public int nAlvl { get; set; }
  public int nTrgr { get; set; }
  public int nProc { get; set; }
  public int nDstID { get; set; }
  public int nTz { get; set; }
  public int nHol { get; set; }
  public int nMpg { get; set; }
  public int nTranLimit { get; set; }
  public int nOperModes { get; set; }
  public int OperType { get; set; }
  public int nLanguages { get; set; }
}