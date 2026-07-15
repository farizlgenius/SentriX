

using SharedKernel.Domain;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface ITimeAdapter
{
      Task CreateHolidayAsync(
          Guid Guid,
          short DeviceComponentId,
          short ComponentId,
           string Name,
          string Mac,
            DateTime Start,
            DateTime End
     );


     Task CreateTimeZoneAsync(
          Guid Guid,
          short DeviceComponentId,
          short TzComponentId,
           string Name,
           string Mac,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalObject> Intervals
     );

     Task UpdateTimeZoneAsync(
          Guid Guid,
          short DeviceComponentId,
          short TzComponentId,
           string Name,
           string Mac,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalObject> Intervals
     );

     Task ClearTimeZoneAsync(
          Guid Guid,
          string Mac
     );

     Task DeleteHolidayAsync(
       short DeviceComponentId,
            int ComponentId,
            string Mac,
            DateTime Start,
            DateTime End
            );

     Task DeleteTimeZoneAsync(
          string Mac,
            short DeviceComponentId,
            short ComponentId
     );

     Task<IEnumerable<OptionDto>> GetTimezoneMode();

     Task UpdateHolidayAsync(
          Guid guid,
           string Name,
           short DeviceComponentId,
           int ComponentId,
          string Mac,
            DateTime Start,
            DateTime End
     );



}