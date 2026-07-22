using Adapter.Amico.Constants;
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

      public async Task CreateGroup(
             Guid Guid,
             string Name,
            List<(Guid DeviceGuid, Guid DoorGuid, Guid TzGuid)> Doors
      )
      {
            foreach (var d in Doors)
            {
                  var amico = await repo.GetAmicoByGuidAsync(d.DeviceGuid);
                  var session = amico.session;

                  if (amico.id == 0)
                        throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), d.DeviceGuid.ToString()));


                  var res = await group.CheckSession(amico.ip, amico.session);

                  if (!res.SessionIsValid)
                  {
                        var news = await group.LoginAsync(amico.ip);
                        session = news.Session;
                        await repo.UpdateSessionByMacAsync(amico.mac, news.Session);
                  }

                  var gres = await group.CreateGroupAsync(
                        amico.ip,
                        session,
                        Name
                  );

                  if(gres.Ids.Count() == 0)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.Group,amico.mac));


                  await repo.AddSlotAsync<Group>(
                              Guid,
                              gres.Ids.ElementAt(0),
                              (g,s) => new Group(g,s)
                        );
                  

                  var arres = await group.CreateAccessRulesAsync(
                        amico.ip,
                        session,
                        Name,
                        0
                  );

                  if(arres.Ids.Count() == 0)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessRule,amico.mac));

                  await repo.AddSlotAsync<AccessRule>(
                              Guid,
                              arres.Ids.ElementAt(0),
                              (g,s) => new AccessRule(g,s)
                        );

                  await group.CreateGroupAccessRuleAsync(
                        amico.ip,
                        session,
                        gres.Ids.ElementAt(0),
                        arres.Ids.ElementAt(0)
                  );

                  await group.CreateAccessRuleTimeZoneAsync(
                        amico.ip,
                        session,
                        await repo.GetSlotIdByGuid<Persistences.Entities.TimeZone>(d.TzGuid),
                        arres.Ids.ElementAt(0)
                  );
            }

      }

      public async Task DeleteGroup(
            Guid DeviceGuid,
            Guid GroupGuid
      )
      {
            var amico = await repo.GetAmicoByGuidAsync(DeviceGuid);
            var session = amico.session;

            if (amico.id == 0)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(Amicos), amico.mac));


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

      public async Task UpdateGroup(
             Guid Guid,
             string Name,
            List<(Guid DeviceGuid, Guid DoorGuid, Guid TzGuid)> Doors
      )
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