using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Constants;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Events.Contract.Command;
using SharedKernel.Domain;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using Time.Contract.DTOs;

namespace Adapter.Aero.Services;

public sealed class AeroTimeService(ITimeCommand time,IMessageBus bus,IAeroRepository repo) : ITimeAdapter
{
      public async Task CreateHolidayAsync(
            string Mac,
            short ScpId,
            short Year,
            short Month,
            short Day,
            string Metadata
            )
      {
            var metadata = JsonSerializer.Deserialize<HolidayMetadata>(Metadata);
            if(metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("HolidayMetadata"));

            var res = time.HolidayConfiguration(
                  Mac,
                  ScpId,
                  Year,
                  Month,
                  Day,
                  metadata.Extend,
                  metadata.TypeMask
                  );

            await bus.SendAsync(new AddCommandEvent(res));


      }

      public async Task CreateTimezoneAsync(
            string Mac,
            short DeviceComponentId,
            short ComponentId,
            short Mode,
            string Active,
            string Deactive,
            List<IntervalDto> Intervals
      )
      {
            var res = time.ExtendedTimezoneActSpecification(
                  Mac,
                  DeviceComponentId,
                  ComponentId,
                  Mode,
                  Active,
                  Deactive,
                  Intervals
                  );

            await bus.SendAsync(new AddCommandEvent(res));


      }

      public async Task DeleteHoliday(
            string Mac,
            short ComponentId,
            short Year,
            short Month,
            short Day,
            string Metadata
      )
      {
            var metadata = JsonSerializer.Deserialize<HolidayMetadata>(Metadata);
            if(metadata == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("HolidayMetadata"));

            var res = time.HolidayConfiguration(
                  Mac,
                  ComponentId,
                  Year,
                  Month,
                  Day,
                  metadata.Extend,
                  0
                  );

            await bus.SendAsync(new AddCommandEvent(res));


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
                  new List<IntervalDto>()
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneMode()
      {
           return await repo.GetTimezoneModeAsync();
      }
}