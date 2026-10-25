using SharedKernel.Enums;
using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IMonitorGroupRepository
{
      CommandResponse ConfigureMonitorPointGroup(
            string mac,
            short scpId,
            short mpgId,
            short mpgCount,
            IEnumerable<(MonitorType,short)> mpgList
      );

      CommandResponse MonitorPointGroupArmDisarmCommand(
            string mac,
            short scpId,
            short mpgId,
            short command,
            short arg1
      );

      CommandResponse MonitorGroupStatusRequest(
            string mac,
            short scpId,
            short mpgId,
            short count
      );
}