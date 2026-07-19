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
      public async Task ClearTimeAsync(Guid Guid,string Mac)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

            await time.ClearTimeAsync(
                  amico.ip,
                  session
            );
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

            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }


           await time.CreateHolidayAsync(
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

      public async Task CreateTimeZoneAsync(
          Guid Guid,
          short DeviceComponentId,
          short TzComponentId,
           string Name,
           string Mac,
            List<IntervalObject> Intervals
     )
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

            // Create Time Zone
            var arr = await time.CreateTimeZoneAsync(
                  amico.ip,
                 session,
                  Name,
                  TzComponentId
            );

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

       
            }

            


      }

      public async Task DeleteHolidayAsync(
            short DeviceComponentId,
            int ComponentId,
            string Mac,
            DateTime Start,
            DateTime End
            )
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

            await time.DeleteHolidayAsync(
                  amico.ip,
                 session,
                 ComponentId
            );




      }

      public async Task DeleteTimeZoneAsync(string Mac, short DeviceComponentId, short TzComponentId,List<short> IntervalComponentId)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

            await time.DeleteTimeZoneAsunc(
                  amico.ip,
                  session,
                  TzComponentId
            );

            foreach(var id in IntervalComponentId)
            {
                  await time.DeleteTimeSpanAsync(
                  amico.ip,
                  session,
                  id
            );
            }

            
      }


      public async Task UpdateHolidayAsync(Guid guid, string Name,short DeviceComponentId,int ComponentId, string Mac, DateTime Start, DateTime End)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }


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

      public async Task UpdateTimeZoneAsync(Guid Guid,short DeviceComponentId, short TzComponentId, string Name, string Mac, List<IntervalObject> Intervals)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await time.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await time.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

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
}