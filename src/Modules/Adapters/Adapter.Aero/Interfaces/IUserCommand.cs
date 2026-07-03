using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Aero.Interfaces;

public interface IUserCommand
{
      CommandResponse AccessDatabaseCardRecords(
            string Mac,
            short ScpId,
            int Flags,
            long CardNumber,
            short IssueCode,
            string Pin,
            List<short> Groups,
            short ApbLoc,
            short UseCount,
            int ActiveDate,
            int DeactiveDate,
            int VacDate,
            short VacDays,
            int TmpDate,
            short TmpDays
            );

}