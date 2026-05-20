using SharedKernel.Model;

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
}