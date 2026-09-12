using Aero.Application.Enums;
using Aero.Application.Interfaces;
using SharedKernel.Model;

namespace Aero.Infrastructure.Repositories;

public sealed class DeviceRepostory : IDeviceRepository
{
      public CommandResponse AccessDatabaseSpecification(string Mac, short ScpId, int nCard, short nAlvl, short nPinDigit, short bIssueCode, short bApbLocation, short bActDate, short bDeactDate, short bVacationDate, short bUpgradeDate, short bUserLevel, short bUseLimit, short bSupportTimeApb, short nTz, short bAssetGroup, short nHostResponseTimeout, short nAvlUse4Arg, short nEscortTimeout, short mMultiCardTimeout)
      {
            throw new NotImplementedException();
      }

      public CommandResponse AsciiCommandAsync(string Mac, short ScpId, string Command)
      {
            throw new NotImplementedException();
      }

      public bool CreateChannel()
      {
            throw new NotImplementedException();
      }

      public CommandResponse DeleteScp(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse DetachScpFromChannel(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse DriverConfiguration(string Mac, short ScpId, short Msp1Number, short PortNumber, short Baudrate, short ReplyTime, short nProtocol, short nDialect)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ElevatorAccessLevelSpecification(string Mac, short ScpId, short MaxEAlvl, short MaxFloor)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ReadsConfiguration(string Mac, short ScpId, WebConfigReadType Type)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ScpDeviceSpecification(string Mac, short ScpId, short nMsp1Port, int nTransaction, short nSio, short nMp, short nCp, short nAcr, short nAlvl, short nTrgr, short nProc, short gmtOffet, short nDstId, short nTz, short nHol, short nMpg, int nTranLimit, short nOperMode, short operType, short nLanguage)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ScpReset(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse ScpStructureStatusRead(string Mac, short ScpId, List<short> StructureList)
      {
            throw new NotImplementedException();
      }

      public CommandResponse SetScpId(string Mac, short ScpId, short To)
      {
            throw new NotImplementedException();
      }

      public CommandResponse SetTransactionLogIndexAsync(string Mac, short ScpId, bool IsEnable)
      {
            throw new NotImplementedException();
      }

      public CommandResponse TimeSet(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }

      public CommandResponse TransactionLogStatusAsync(string Mac, short ScpId)
      {
            throw new NotImplementedException();
      }
}