using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IUserRepository
{
      CommandResponse AccessDatabaseCardRecords(
            string mac,
            short scpId,
            short flags,
            int cardNumber,
            short issueCode,
            string pin,
            List<short> alvl,
            short apbLoc,
            int useCount,
            DateTime actTime,
            DateTime? dactTime
      );

      CommandResponse CardDelete(string mac,short scpId,int cardNumber);
}