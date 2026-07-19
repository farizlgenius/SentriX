using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using Adapter.Amico.Persistences.Entities;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Adapter.Amico.Adapters;

public sealed class AmicoGroupAdapter(
      IGroupCommand group,
      IAmicoRepository repo
) : IAmicoGroupAdapter
{

      public async Task CreateGroup(string Name, short ComponentId, List<(string Mac, short DeviceComponentId, short DoorComponentId, short TimeZoneComponentId)> Doors)
      {
            foreach (var d in Doors)
            {
                  var amico = await repo.GetAmicoByMacAsync(d.Mac);
                  var session = amico.session;

                  if (amico.id == 0)
                        throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), d.Mac));


                  var res = await group.CheckSession(amico.ip, amico.session);

                  if (!res.SessionIsValid)
                  {
                        var news = await group.LoginAsync(amico.ip);
                        session = news.Session;
                        await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
                  }

                  await group.CreateGroupAsync(
                        amico.ip,
                        session,
                        ComponentId,
                        Name
                  );

                  await group.CreateAccessRulesAsync(
                        amico.ip,
                        session,
                        ComponentId,
                        Name,
                        0
                  );

                  await group.CreateGroupAccessRuleAsync(
                        amico.ip,
                        session,
                        ComponentId,
                        ComponentId
                  );

                  await group.CreateAccessRuleTimeZoneAsync(
                        amico.ip,
                        session,
                        d.TimeZoneComponentId,
                        ComponentId
                  );
            }

      }

      public async Task DeleteGroup(string Mac, short DeviceComponentId, short ComponentId)
      {
            var amico = await repo.GetAmicoByMacAsync(Mac);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), Mac));


            var res = await group.CheckSession(amico.ip, amico.session);

            if (!res.SessionIsValid)
            {
                  var news = await group.LoginAsync(amico.ip);
                  session = news.Session;
                  await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
            }

            await group.DeleteAccessRuleTimeZoneAsync(
                        amico.ip,
                        session,
                        ComponentId
                  );

            await group.DeleteGroupAccessRuleAsync(
                       amico.ip,
                       session,
                       ComponentId,
                       ComponentId
                 );

            await group.DeleteGroupAsync(
                        amico.ip,
                        session,
                        ComponentId
                  );

            await group.DeleteAccessRuleAsync(
                  amico.ip,
                  session,
                  ComponentId
            );




      }

      public async Task UpdateGroup(string Name, short ComponentId, List<(string Mac, short DeviceComponentId, short DoorComponentId, short TimeZoneComponentId)> Doors)
      {
            foreach (var d in Doors)
            {
                  var amico = await repo.GetAmicoByMacAsync(d.Mac);
                  var session = amico.session;

                  if (amico.id == 0)
                        throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), d.Mac));


                  var res = await group.CheckSession(amico.ip, amico.session);

                  if (!res.SessionIsValid)
                  {
                        var news = await group.LoginAsync(amico.ip);
                        session = news.Session;
                        await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
                  }

                  await group.UpdateGroupAsync(
                        amico.ip,
                        session,
                        ComponentId,
                        Name
                  );

                  await group.UpdateAccessRulesAsync(
                        amico.ip,
                        session,
                        ComponentId,
                        Name,
                        0
                  );

                  await group.UpdateGroupAccessRuleAsync(
                        amico.ip,
                        session,
                        ComponentId,
                        ComponentId
                  );

                  await group.UpdateAccessRuleTimeZoneAsync(
                        amico.ip,
                        session,
                        d.TimeZoneComponentId,
                        ComponentId
                  );
            }

      }
}