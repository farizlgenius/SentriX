using System;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Command;

public class BaseCommand
{
      protected bool Send(short command, IConfigCommand cfg)
    {
        SCPConfig scp = new SCPConfig();
        bool success = scp.scpCfgCmndEx(command, cfg);
        return success;
    }
}
