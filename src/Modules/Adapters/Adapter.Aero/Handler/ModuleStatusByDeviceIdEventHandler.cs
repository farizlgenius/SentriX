// using System;
// using Adapter.Abstraction.Command;
// using Adapter.Abstraction.Events;
// using Adapter.Aero.Interfaces;
// using Adapter.Aero.Persistences.Entities;
// using AeroAdapter.Application.Interfaces;
// using SharedKernel.Messaging;

// namespace Adapter.Aero.Handler;

// public sealed class ModuleStatusByModuleIdEventHandler(
//       IModuleCommand command,
//       IAeroRepository repo) : ICommandHandler<ModuleStatusCommand>
// {
//       public async Task HandleAsync(ModuleStatusCommand com, CancellationToken ct)
//       {
//             var slots = await repo.GetSlotByGuidAsync<SioSlot>(com.moduleGuid);
//             var deviceSlot = await repo.GetScpSlotByGuidAsync(slots.device_guid);
//             command.SioStatusRequest(deviceSlot.mac,(short)deviceSlot.slot_id,slots.slot_id,1);
//       }
// }
