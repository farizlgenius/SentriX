using System;
using Adapter.Aero.Persistences.Entities;
using SharedKernel.Model;


namespace AeroAdapter.Application.Interfaces;

public interface IInputCommand
{
      CommandResponse InputPointSpecification(
            string Mac,
            short ScpId,
            short SioNumber,
            short InputNumber,
            short IcvtNumber,
            short Debounce,
            short HoldTime
      );

      public CommandResponse MonitorPointConfiguration(
            string Mac,
            short ScpId,
            short MpNumber,
            short SioNumber,
            short IpNo,
            short LfCode,
            short Mode,
            short DelayEntry,
            short DelayExit
      );

      public CommandResponse MonitorPointMask(
            string Mac,
            short ScpId,
            short MpNumber,
            bool IsMask 
      );

      public CommandResponse ConfigureMonitorPointGroup(
            string Mac,
            short ScpId,
            short MpgNumber,
            List<(short Type,short Number)> MpList
      );
}
