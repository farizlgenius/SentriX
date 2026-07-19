using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Aero.Interfaces;

public interface IUserCommand
{


      CommandResponse AccessDatabaseCardRecords(
             string Mac,
            short ScpId,
            long CardNumber,
            string Pin,
            List<short> Groups,
            int ActiveDate,
            int DeactiveDate
      );

      CommandResponse CardDelete(
            string Mac,
            short ScpId,
            long CardNumber
      );

}