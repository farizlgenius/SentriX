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
                  var session = await group.CheckSessionAsync(amico.ip, amico.session);

                  // Create Access Rule Here
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

                  // Create Group Here

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
                  

                 

                  var garres = await group.CreateGroupAccessRuleAsync(
                        amico.ip,
                        session,
                        gres.Ids.ElementAt(0),
                        arres.Ids.ElementAt(0)
                  );

                   if(garres.Ids.Count() == 0)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.GroupAccessRule,amico.mac));


                  var artzres =  await group.CreateAccessRuleTimeZoneAsync(
                        amico.ip,
                        session,
                        await repo.GetSlotIdByGuidAsync<Persistences.Entities.TimeZone>(d.TzGuid),
                        arres.Ids.ElementAt(0)
                  );

                   if(artzres.Ids.Count() == 0)
                        throw new Exception(MessageHelper.Command.Unsuccess(CommandConstant.AccessRuleTimeZone,amico.mac));

            }

      }

      public async Task DeleteGroup(
            Guid GroupGuid,
            List<Guid> DeviceGuids
      )
      {
            foreach (var DeviceGuid in DeviceGuids)
            {
                  var amico = await repo.GetAmicoByGuidAsync(DeviceGuid);
                  var session = await group.CheckSessionAsync(amico.ip, amico.session);

                  var groupSlot = await repo.GetSlotByGuidAsync<Group>(GroupGuid);
                  var accessRuleSlot = await repo.GetSlotByGuidAsync<AccessRule>(groupSlot.access_rule_guid);


                  await group.DeleteAccessRuleTimeZoneAsync(
                              amico.ip,
                              session,
                              accessRuleSlot.slot_id
                        );


                  await group.DeleteGroupAccessRuleAsync(
                             amico.ip,
                             session,
                             groupSlot.slot_id,
                             accessRuleSlot.slot_id
                       );

                  await group.DeleteGroupAsync(
                              amico.ip,
                              session,
                              groupSlot.slot_id
                        );

                  await group.DeleteAccessRuleAsync(
                        amico.ip,
                        session,
                        accessRuleSlot.slot_id
                  );
            }

      }

      public async Task UpdateGroup(
             Guid Guid,
             string Name,
            List<(Guid DeviceGuid, Guid DoorGuid, Guid TzGuid)> Doors
      )
      {
            foreach (var d in Doors)
            {
                  var amico = await repo.GetAmicoByGuidAsync(d.DeviceGuid);
                  var session = await group.CheckSessionAsync(amico.ip, amico.session);

                  var groupSlot = await repo.GetSlotByGuidAsync<Group>(Guid);
                  var accessRuleSlot = await repo.GetSlotByGuidAsync<AccessRule>(groupSlot.access_rule_guid);

                  await group.UpdateGroupAsync(
                        amico.ip,
                        session,
                        groupSlot.slot_id,
                        Name
                  );

                  await group.UpdateAccessRulesAsync(
                        amico.ip,
                        session,
                        accessRuleSlot.slot_id,
                        Name,
                        0
                  );

                  await group.UpdateGroupAccessRuleAsync(
                        amico.ip,
                        session,
                        groupSlot.slot_id,
                        accessRuleSlot.slot_id
                  );

                  await group.UpdateAccessRuleTimeZoneAsync(
                        amico.ip,
                        session,
                        await repo.GetSlotIdByGuidAsync<Persistences.Entities.TimeZone>(d.TzGuid),
                        accessRuleSlot.slot_id
                  );
            }

      }
}