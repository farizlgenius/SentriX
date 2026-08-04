using Adapter.Amico.Constants;
using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Objects;
using Adapter.Amico.Persistences.Entities;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Model;
using Time.Contract.DTOs;

namespace Adapter.Amico.Adapters;

public sealed class AmicoTimeAdapter(
      ITimeCommand time,
      IAmicoRepository repo) : IAmicoTimeAdapter
{
      public async Task ClearTimeAsync(
            Guid TzGuid,
          List<Guid> DeviceGuids
      )
      {
            var amico = await repo.GetAmicoByGuidAsync(TzGuid);
            var session = await time.CheckSessionAsync(amico.ip, amico.session);

            await time.ClearTimeAsync(
                  amico.ip,
                  session
            );
      }


      public async Task CreateHolidayAsync(
            Guid HolidayGuid,
         List<Guid> DeviceGuids,
          string Name,
           DateTime Start,
           DateTime End
      )
      {
            foreach (var g in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(g);
                  var session = await time.CheckSessionAsync(amico.ip, amico.session);


                  var response = await time.CreateHolidayAsync(
                         amico.ip,
                        session,
                         Name,
                         (int)DateTimeHelper.DateTimeToElapeSecond(Start),
                         (int)DateTimeHelper.DateTimeToElapeSecond(End),
                         1,
                         1,
                         1,
                         0
                   );

                  if (response.Ids.Count == 0)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.Holiday, amico.mac));

                  await repo.AddSlotAsync<Persistences.Entities.Holiday>(
                  HolidayGuid,
                  amico.guid,
                  response.Ids[0],
                  (g, d, s) => new Persistences.Entities.Holiday(g, d, s)
            );
            }




      }


      public async Task CreateTimeZoneAsync(
            Guid TzGuid,
           string Name,
            List<IntervalObject> Intervals,
            List<Guid> DeviceGuids
            )
      {
            foreach (var g in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(g);
                  var session = await time.CheckSessionAsync(amico.ip, amico.session);

                  // Create Time Zone
                  var response = await time.CreateTimeZoneAsync(
                        amico.ip,
                       session,
                        Name
                  );

                  await repo.AddSlotAsync<Persistences.Entities.TimeZone>(
                        TzGuid,
                        g,
                        response.Ids[0],
                        (g,d,s) => new Persistences.Entities.TimeZone(g,d,s)
                  );

                  if (response.Ids.Count == 0)
                        throw new Exception(CommandConstant.TimeZone);

                  foreach (var interval in Intervals)
                  {

                        // Create Time Span
                        var arr = await time.CreateTimeSpanAsync(
                               amico.ip,
                               session,
                               response.Ids[0],
                              interval.Start,
                              interval.End,
                               interval.Sun ? 1 : 0,
                               interval.Mon ? 1 : 0,
                               interval.Tue ? 1 : 0,
                               interval.Wed ? 1 : 0,
                               interval.Thu ? 1 : 0,
                               interval.Fri ? 1 : 0,
                               interval.Sat ? 1 : 0,
                               1,
                               1,
                               1
                         );

                        if (arr.Ids.Count == 0)
                              throw new Exception(CommandConstant.TimeSpan);


                        await repo.AddSlotAsync<Persistences.Entities.TimeSpan>(
                              TzGuid,
                              g,
                              arr.Ids.ElementAt(0),
                              (g,d,s) => new Persistences.Entities.TimeSpan(g,d,s)
                        );


                  }
            }


      }

      public async Task DeleteHolidayAsync(
            Guid HolidayGuid,
          List<Guid> DeviceGuids,
            DateTime Start,
            DateTime End
            )
      {
            foreach (var g in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(g);
                  var session = await time.CheckSessionAsync(amico.ip, amico.session);

                  var hol = await repo.GetSlotIdByGuidAsync<Persistences.Entities.Holiday>(HolidayGuid);

                  await time.DeleteHolidayAsync(
                        amico.ip,
                       session,
                       hol
                  );

            }

      }


      public async Task DeleteTimeZoneAsync(
            Guid TzGuid,
          List<short> IntervalComponentId,
          List<Guid> DeviceGuids
      )
      {
            foreach (var g in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(g);
                  var session = await time.CheckSessionAsync(amico.ip, amico.session);

                  var tz = await repo.GetSlotIdByGuidAsync<Persistences.Entities.TimeZone>(TzGuid);

                  await time.DeleteTimeZoneAsunc(
                        amico.ip,
                        session,
                        tz
                  );

                  foreach (var id in IntervalComponentId)
                  {
                        await time.DeleteTimeSpanAsync(
                        amico.ip,
                        session,
                        id
                  );
                  }
                  }
            


      }



      public async Task UpdateHolidayAsync(
            Guid HolidayGuid, 
            List<Guid> DeviceGuids, 
              string Name, 
            DateTime Start, 
            DateTime End
      )
      {

            foreach (var g in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(g);
                  var session = await time.CheckSessionAsync(amico.ip, amico.session);

                  var ComponentId = await repo.GetSlotIdByGuidAsync<Persistences.Entities.Holiday>(HolidayGuid);


                  await time.UpdateHolidayAsync(
                        amico.ip,
                       session,
                        Name,
                        ComponentId,
                        (int)DateTimeHelper.DateTimeToElapeSecond(Start),
                        (int)DateTimeHelper.DateTimeToElapeSecond(End),
                        1,
                        1,
                        1,
                        0
                  );
            }



      }


      public async Task UpdateTimeZoneAsync(
            Guid TzGuid,
          string Name,
          List<IntervalObject> Intervals,
          List<Guid> DeviceGuids
      )
      {
            foreach (var g in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(g);
                  var session = await time.CheckSessionAsync(amico.ip, amico.session);

                  var TzComponentId = await repo.GetSlotIdByGuidAsync<Persistences.Entities.TimeZone>(TzGuid);

                  // Create Time Zone
                  var arr = await time.UpdateTimeZoneAsync(
                        amico.ip,
                       session,
                        Name,
                        TzComponentId
                  );


                  var intervalComponentIds = await repo.GetSlotIdByGuidAsync<Persistences.Entities.TimeSpan>(TzGuid);

                  foreach (var interval in Intervals)
                  {

                        var intervalComponentId = await repo.GetSlotIdByGuidAsync<Persistences.Entities.TimeSpan>(TzGuid);

                        // Create Time Span
                        await time.UpdateTimeSpanAsync(
                               amico.ip,
                               session,
                               TzComponentId,
                               intervalComponentId,
                              interval.Start,
                              interval.End,
                               interval.Sun ? 1 : 0,
                               interval.Mon ? 1 : 0,
                               interval.Tue ? 1 : 0,
                               interval.Wed ? 1 : 0,
                               interval.Thu ? 1 : 0,
                               interval.Fri ? 1 : 0,
                               interval.Sat ? 1 : 0,
                               1,
                               1,
                               1
                         );

                  }
            }

      }


}