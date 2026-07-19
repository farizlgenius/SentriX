using Adapter.Aero.Interfaces;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroSettingAdapter(ISettingCommand writer,IMessageBus bus,IAeroRepository repo) : IAeroSettingAdapter
{
      public async Task CardFormatConfiguration(
            Guid guid,
            short Fac,
            short Offset,
            short FunctionId,
            short Flag,
            short Bits,
            short PeLn,
            short PeLoc,
            short PoLn,
            short PoLoc,
            short FcLn,
            short FcLoc,
            short ChLn,
            short ChLoc,
            short IcLn,
            short IcLoc
      )
      {
            var slots = await repo.GetScpSlotByGuidAsync(guid);
            var res = writer.CardFormatterConfiguration(
                  slots.mac,
                  slots.slot_id,
                  ComponentId,
                  Fac,
                  Offset,
                  FunctionId,
                  Flag,
                  Bits,
                  PeLn,
                  PeLoc,
                  PoLn,
                  PoLoc,
                  FcLn,
                  FcLoc,
                  ChLn,
                  ChLoc,
                  IcLn,
                  IcLoc
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}