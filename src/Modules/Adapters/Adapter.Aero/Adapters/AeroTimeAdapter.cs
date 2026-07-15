using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Device.Contract.Queries;
using Events.Contract.Command;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Aero.Adapters;

public sealed class AeroTimeAdapter(ITimeCommand time,IMessageBus bus,IAeroRepository repo) : IAeroTimeAdapter
{
      public Task ClearTimeZoneAsync(Guid Guid, string Mac)
      {
            throw new NotImplementedException();
      }

      public async Task CreateHolidayAsync(
            Guid Guid,
            short DeviceComponentId,
            short ComponentId,
            string Name,
            string Mac,
            DateTime Start,
            DateTime End
            )
      {

            var dates = DateTimeHelper.ExtractDateFromStartEndDateTime(Start, End);

            foreach (var date in dates)
            {
                  var res = time.HolidayConfiguration(
                  Mac,
                  DeviceComponentId,
                  date.Year,
                  date.Month,
                  date.Day,
                  0,
                  1
                  );

                  await bus.SendAsync(new AddCommandEvent(res));

                  
            }


      }

      public async Task CreateTimeZoneAsync(
           Guid Guid,
           short DeviceComponentId,
          short TzComponentId,
           string Name,
           string Mac,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalObject> Intervals
      )
      {
            
    
            var res = time.ExtendedTimezoneActSpecification(
                  Mac,
                  DeviceComponentId,
                  TzComponentId,
                  Mode,
                  Active,
                  Deactive,
                  Intervals
           );

            await bus.SendAsync(new AddCommandEvent(res));


      }



      public async Task DeleteHolidayAsync(
            short DeviceComponentId,
            int ComponentId,
            string Mac,
            DateTime Start,
            DateTime End
      )
      {
            var dates = DateTimeHelper.ExtractDateFromStartEndDateTime(Start,End);

            foreach(var date in dates)
            {
                  var res = time.HolidayConfiguration(
                  Mac,
                  DeviceComponentId,
                  date.Year,
                  date.Month,
                  date.Day,
                  0,
                  0
                  );

            await bus.SendAsync(new AddCommandEvent(res));
            }

      

      }

      public async Task DeleteTimezone(
            string Mac,
            short DeviceComponentId,
            short ComponentId
            )
      {
            var res = time.ExtendedTimezoneActSpecification(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  0,
                  string.Empty,
                  string.Empty,
                  new List<IntervalObject>()
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }

      public async Task DeleteTimeZoneAsync(string Mac, short DeviceComponentId, short ComponentId)
      {
            var ScpId = await bus.QueryAsync(new ComponentIdByMacQuery(Mac));

           var res = time.ExtendedTimezoneActSpecification(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  0,
                  string.Empty,
                  string.Empty,
                  new List<IntervalObject>()
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneMode()
      {
           return await repo.GetTimezoneModeAsync();
      }

      public async Task UpdateHolidayAsync(Guid guid, string Name,short DeviceComponentId, int ComponentId, string Mac, DateTime Start, DateTime End)
      {


            var dates = DateTimeHelper.ExtractDateFromStartEndDateTime(Start,End);

            

            foreach(var date in dates)
            {

                  var res = time.HolidayConfiguration(
                  Mac,
                  DeviceComponentId,
                  date.Year,
                  date.Month,
                  date.Day,
                  0,
                  1
                  );

                  await bus.SendAsync(new AddCommandEvent(res));
            }


      }

      public async Task UpdateTimeZoneAsync(Guid Guid,short DeviceComponentId, short TzComponentId, string Name, string Mac, short Mode, string Active, string Deactive, List<IntervalObject> Intervals)
      {
    
            var res = time.ExtendedTimezoneActSpecification(
                  Mac,
                  DeviceComponentId,
                  TzComponentId,
                  Mode,
                  Active,
                  Deactive,
                  Intervals
           );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}