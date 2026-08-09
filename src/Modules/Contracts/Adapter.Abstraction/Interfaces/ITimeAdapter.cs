

namespace Adapter.Abstraction.Interfaces;

public interface ITimeAdapter
{

     // Holiday
     Task CreateHolidayAsync(
          Guid HolidayGuid,
         List<Guid> DeviceGuids,
          string Name,
           DateTime Start,
           DateTime End
    );

     Task DeleteHolidayAsync(
          Guid HolidayGuid,
          List<Guid> DeviceGuids,
       DateTime Start,
       DateTime End
       );


     Task UpdateHolidayAsync(
          Guid HolidayGuid,
          List<Guid> DeviceGuids, 
          string Name, 
          DateTime Start, 
          DateTime End);


     // Time Zone


     // Task CreateTimeZoneAsync(
     //       Guid TzGuid,
     //       string Name,
     //        List<IntervalObject> Intervals,
     //        List<Guid> DeviceGuids
     //  );

     // Task UpdateTimeZoneAsync(
     //      Guid TzGuid,
     //      string Name, 
     //      List<IntervalObject> Intervals,
     //      List<Guid> DeviceGuids
     //      );

     Task DeleteTimeZoneAsync(
          Guid TzGuid,
          List<short> IntervalComponentId,
          List<Guid> DeviceGuids
          );


     // Reset Delete All


     Task ClearTimeAsync(
          Guid TzGuid,
          List<Guid> DeviceGuids
     );


}