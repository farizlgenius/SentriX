using System;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Command;

public class BaseCommand
{
    public bool IsBypass { get; set; } = false;
      protected bool Send(short command, IConfigCommand cfg)
    {
        // if(IsBypass)
        //     return true;

        // SCPConfig scp = new SCPConfig();
        // bool success = scp.scpCfgCmndEx(command, cfg);
        // return success;
        return true;
    }
}
