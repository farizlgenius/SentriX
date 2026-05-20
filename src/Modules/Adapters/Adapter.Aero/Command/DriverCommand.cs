using System;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;

namespace Adapter.Aero.Command;

public sealed class DriverCommand(ILogger<DriverCommand> logger) : BaseCommand, IDriverCommand
{
      public bool SystemLevelSpecification()
      {

            CC_SYS c = new CC_SYS();
            c.nPorts = 1024;
            c.nScps = 1024;
            c.nTimezones = 0;
            c.nHolidays = 0;
            c.bDirectMode = 1;
            c.debug_rq = 0;
            for (int i = 0; i < c.nDebugArg.Length; i++)
            {
                  c.nDebugArg[i] = 0;
            }
            var result = Send((short)enCfgCmnd.enCcSystem, c);
            if (result)
                  logger.LogInformation("System level specification command sent successfully.");
            return result;
      }





}