using Adapter.Aero.Interfaces;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences.Entities;
using Events.Contract.Command;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroSettingAdapter(ISettingCommand writer,IMessageBus bus,IAeroRepository repo) : IAeroSettingAdapter
{
      public async Task CardFormatConfiguration(
            Guid Guid,
            string Metadata
      )
      {
            var meta = JsonHelper.Deserialize<CardFormatMetadata>(Metadata);
            if(meta is null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed(nameof(CardFormatMetadata)));

            var deviceSlot = await repo.GetScpSlotByGuidAsync(Guid);
            var slot = await repo.GetFreeSlotAsync<CardFormatSlot>();
            var res = writer.CardFormatterConfiguration(
                  deviceSlot.mac,
                  (short)deviceSlot.slot_id,
                  (short)slot,
                  meta.Fac,
                  meta.Offset,
                  meta.FunctionId,
                  meta.Flag,
                  meta.Bits,
                  meta.PeLn,
                  meta.PeLoc,
                  meta.PoLn,
                  meta.PoLoc,
                  meta.FcLn,
                  meta.FcLoc,
                  meta.ChLn,
                  meta.ChLoc,
                  meta.IcLn,
                  meta.IcLoc
                  );

            await bus.SendAsync(new AddCommandEvent(res));

            await repo.InsertSlotAsync<CardFormatSlot>(
                  Guid,
                  slot
            );
      }
}