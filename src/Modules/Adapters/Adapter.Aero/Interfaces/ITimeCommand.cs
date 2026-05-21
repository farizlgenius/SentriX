using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Aero.Interfaces;

public interface ITimeCommand
{
      CommandResponse HolidayConfiguration(
            string Mac,
            short ScpId,
            short Year,
            short Month,
            short Day,
            short Extend,
            short TypeMask
            );

      CommandResponse ExtendedTimezoneActSpecification(
            string Mac,
            short ScpId,
            short TzNumber,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalDto> Intervals
      );
}