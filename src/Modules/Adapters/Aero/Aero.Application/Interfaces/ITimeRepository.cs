using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface ITimeRepository
{
      CommandResponse ExtendedTimeZoneActSpecification(
           string mac, short scpId, short tzNumber, short mode,List<(short i_day,short start,short end)> intervalDetail
      );
}