using System;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Persistences;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.Extensions.Logging;

namespace Adapter.Aero.Writer;

public sealed class DriverWriter(AeroDbContext context, ILogger<DriverWriter> logger) : BaseWriter, IDriverWriter
{
      public bool SystemLevelSpecification()
      {
            var data = context.SystemLevelSpecifications.OrderByDescending(x => x.id).FirstOrDefault();
            if (data == null)
                  return false;

            CC_SYS c = new CC_SYS();
            c.nPorts = data.n_ports;
            c.nScps = data.n_scps;
            c.nTimezones = data.n_timezones;
            c.nHolidays = data.n_holidays;
            c.bDirectMode = data.b_direct_mode;
            c.debug_rq = data.debug_rq;
            for (int i = 0; i < c.nDebugArg.Length; i++)
            {
                  c.nDebugArg[i] = data.n_debug_arg;
            }
            var result = Send((short)enCfgCmnd.enCcSystem, c);
            if (result)
                  logger.LogInformation("System level specification command sent successfully.");
            return result;
      }





}