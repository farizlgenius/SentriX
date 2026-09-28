using Adapter.Contract.Interfaces;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Core.Contract.Commands.Events;
using Core.Contract.DTOs.Time;
using Core.Contract.Queries.ComponentMapping;
using SharedKernel.Constants;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class TimeService(
      IMessageBus bus,
      ITimeRepository repo
      ) : ITimeAdapter
{

      public async Task TimeZone(
            string mac,
            string ip,
            short externalId,
            short tzExternalId,
            List<IntervalDto> intervals,
            CancellationToken ct = default)
      {

            var res = repo.ExtendedTimeZoneActSpecification(
                  mac,
                  externalId,
                  tzExternalId,
                  2,
                  intervals.Select(x =>
                  (
                        (short)UtilitiesHelper.ConvertDayToBinary(
                              x.Days.Sunday,
                              x.Days.Monday,
                              x.Days.Tuesday,
                              x.Days.Wednesday,
                              x.Days.Thursday,
                              x.Days.Friday,
                              x.Days.Saturday
                              ),
                        (short)UtilitiesHelper.TimeOnlyToInt(x.Start),
                        (short)UtilitiesHelper.TimeOnlyToInt(x.End)
                        )).ToList()
            );

            await bus.SendAsync(new AdapterEventCommand(res));
      }
}

