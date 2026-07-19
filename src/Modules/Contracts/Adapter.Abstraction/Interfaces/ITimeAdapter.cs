

using SharedKernel.Domain;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface ITimeAdapter
{

     // Holiday
     Task CreateHolidayAsync(
         Guid Guid,
          string Name,
           DateTime Start,
           DateTime End
    );

     Task DeleteHolidayAsync(
          Guid Guid,
       DateTime Start,
       DateTime End
       );


     Task UpdateHolidayAsync(Guid DeviceGuid,DateTime Start, DateTime End);


     // Time Zone


     Task CreateTimeZoneAsync(
           Guid DeviceGuid,
           Guid TzGuid,
           string Name,
            List<IntervalObject> Intervals
      );

     Task UpdateTimeZoneAsync(Guid DeviceGuid,Guid TzGuid, string Name, List<IntervalObject> Intervals);

     Task DeleteTimeZoneAsync(Guid DeviceGuid,Guid TzGuid, List<short> IntervalComponentId);


     // Reset Delete All


     Task ClearTimeAsync(
          Guid Guid
     );


}