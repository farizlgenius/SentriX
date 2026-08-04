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
      public async Task ClearTimeAsync(Guid Guid)
      {
            var amico = await repo.GetAmicoByGuidAsync(Guid);
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


      public async Task CreateTimeZoneAsync(Guid DeviceGuid, Guid TzGuid, string Name, List<IntervalObject> Intervals)
      {
            var amico = await repo.GetAmicoByGuidAsync(DeviceGuid);
            var session = await time.CheckSessionAsync(amico.ip, amico.session);

            // Create Time Zone
            var response = await time.CreateTimeZoneAsync(
                  amico.ip,
                 session,
                  Name
            );

            await repo.AddSlotAsync<Persistences.Entities.TimeZone>(
                  TzGuid,
                  response.Ids[0],
                  (g, s) => new Persistences.Entities.TimeZone(g, s)
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
                        arr.Ids.ElementAt(0),
                        (g, s) => new Persistences.Entities.TimeSpan(g, s)
                  );


            }
      }

      public async Task DeleteHolidayAsync(
            Guid Guid,
            Guid DeviceGuid,
            DateTime Start,
            DateTime End
            )
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = await time.CheckSessionAsync(amico.ip, amico.session);

            await time.DeleteHolidayAsync(
                  amico.ip,
                 session,
                 ComponentId
            );




      }

      public Task DeleteHolidayAsync(Guid Guid, DateTime Start, DateTime End)
      {
            throw new NotImplementedException();
      }

      public async Task DeleteTimeZoneAsync(string Mac, short DeviceComponentId, short TzComponentId, List<short> IntervalComponentId)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = await time.CheckSessionAsync(amico.ip, amico.session);

            await time.DeleteTimeZoneAsunc(
                  amico.ip,
                  session,
                  TzComponentId
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

      public Task DeleteTimeZoneAsync(Guid DeviceGuid, Guid TzGuid, List<short> IntervalComponentId)
      {
            throw new NotImplementedException();
      }

      public async Task UpdateHolidayAsync(Guid guid, string Name, short DeviceComponentId, int ComponentId, string Mac, DateTime Start, DateTime End)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = await time.CheckSessionAsync(amico.ip, amico.session);


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

      public Task UpdateHolidayAsync(Guid DeviceGuid, DateTime Start, DateTime End)
      {
            throw new NotImplementedException();
      }

      public async Task UpdateTimeZoneAsync(Guid Guid, short DeviceComponentId, short TzComponentId, string Name, string Mac, List<IntervalObject> Intervals)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
           var session = await time.CheckSessionAsync(amico.ip, amico.session);

            // Create Time Zone
            var arr = await time.CreateTimeZoneAsync(
                  amico.ip,
                 session,
                  Name,
                  TzComponentId
            );

            int i = 0;
            foreach (var interval in Intervals)
            {

                  // Create Time Span
                  await time.CreateTimeSpanAsync(
                         amico.ip,
                         session,
                         TzComponentId,
                         interval.ComponentId,
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

                  i++;
            }
      }

      public Task UpdateTimeZoneAsync(Guid DeviceGuid, Guid TzGuid, string Name, List<IntervalObject> Intervals)
      {
            throw new NotImplementedException();
      }
}