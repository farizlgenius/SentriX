

using SharedKernel.Domain;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface ITimeAdapter
{

     // Holiday
      Task CreateHolidayAsync(
          Guid Guid,
          short DeviceComponentId,
          short ComponentId,
           string Name,
          string Mac,
            DateTime Start,
            DateTime End
     );

          Task DeleteHolidayAsync(
       short DeviceComponentId,
            int ComponentId,
            string Mac,
            DateTime Start,
            DateTime End
            );


                 Task UpdateHolidayAsync(
          Guid guid,
           string Name,
           short DeviceComponentId,
           int ComponentId,
          string Mac,
            DateTime Start,
            DateTime End
     );


     // Time Zone


     Task CreateTimeZoneAsync(
          Guid Guid,
          short DeviceComponentId,
          short TzComponentId,
           string Name,
           string Mac,
            List<IntervalObject> Intervals
     );

     Task UpdateTimeZoneAsync(
          Guid Guid,
          short DeviceComponentId,
          short TzComponentId,
           string Name,
           string Mac,
            List<IntervalObject> Intervals
     );

          Task DeleteTimeZoneAsync(
          string Mac,
            short DeviceComponentId,
            short ComponentId,
            List<short> IntervalComponentId
     );


     // Reset Delete All


     Task ClearTimeAsync(
          Guid Guid,
          string Mac
     );








}