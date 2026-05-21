

using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface ITimeAdapter
{
     Task CreateHolidayAsync(
       string Mac,
            short ScpId,
            short Year,
            short Month,
            short Day,
            string Metadata
     );  

     Task CreateTimezoneAsync(
           string Mac,
            short DeviceComponentId,
            short ComponentId,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalDto> Intervals
     );

     Task DeleteHoliday(
           string Mac,
            short ComponentId,
            short Year,
            short Month,
            short Day,
            string Metadata
     );

     Task DeleteTimezone(
          string Mac,
            short DeviceComponentId,
            short ComponentId
     );

     Task<IEnumerable<OptionDto>> GetTimezoneMode();
}