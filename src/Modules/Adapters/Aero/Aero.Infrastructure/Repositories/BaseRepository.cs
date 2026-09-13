using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Repositories;

public class BaseRepository : IBaseRepository
{
  public bool IsBypass { get; set; } = false;
  public bool Send(short command, object cfg)
  {
    if (IsBypass)
      return true;

    SCPConfig scp = new SCPConfig();
    bool success = scp.scpCfgCmndEx(command, (IConfigCommand)cfg);
    return success;

  }
}