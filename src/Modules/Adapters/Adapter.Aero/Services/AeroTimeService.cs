using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Helpers;
using Adapter.Aero.Interfaces;
using Adapter.Aero.Model.Metadata;
using Events.Contract.Command;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroTimeService(ITimeCommand time,IMessageBus bus) : ITimeAdapter
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
}