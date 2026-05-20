using System;
using Adapter.Aero.Enums;
using Adapter.Aero.Persistences.Entities;
using SharedKernel.Model;



namespace AeroAdapter.Application.Interfaces;

public interface IScpCommand
{
      // Below Command need to reset controller if change
      CommandResponse ScpDeviceSpecification(
            string Mac,
            short ScpId,
            short nMsp1Port,
            int nTransaction,
            short nSio,
            short nMp,
            short nCp,
            short nAcr,
            short nAlvl,
            short nTrgr,
            short nProc,
            short gmtOffet,
            short nDstId,
            short nTz,
            short nHol,
            short nMpg,
            int nTranLimit,
            short nOperMode,
            short operType,
            short nLanguage
      );
      CommandResponse AccessDatabaseSpecification(
            string Mac,
            short ScpId,
            int nCard,
            short nAlvl,
            short nPinDigit,
            short bIssueCode,
            short bApbLocation,
            short bActDate,
            short bDeactDate,
            short bVacationDate,
            short bUpgradeDate,
            short bUserLevel,
            short bUseLimit,
            short bSupportTimeApb,
            short nTz,
            short bAssetGroup,
            short nHostResponseTimeout,
            short nAvlUse4Arg,
            short nEscortTimeout,
            short mMultiCardTimeout
      );

      // End

      CommandResponse TimeSet(string Mac,short ScpId);
      
      bool CreateChannel();
      CommandResponse DriverConfiguration(
            string Mac,
            short ScpId,
            short Msp1Number,
            short PortNumber,
            short Baudrate,
            short ReplyTime,
            short nProtocol,
            short nDialect
      );
      CommandResponse ReadsConfiguration(string Mac,short ScpId,WebConfigReadType Type);
     CommandResponse ScpStructureStatusRead(string Mac,short ScpId,List<short> StructureList);
     CommandResponse ElevatorAccessLevelSpecification(string Mac,short ScpId,short MaxEAlvl,short MaxFloor);
      CommandResponse ScpReset(string Mac,short ScpId);
      CommandResponse AsciiCommandAsync(string Mac,short ScpId,string Command);
      CommandResponse SetScpId(string Mac,short ScpId,short To);

}
