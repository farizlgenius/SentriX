using Adapter.Contract.Interfaces;
using Aero.Application.Interfaces;
using Aero.Application.Metadata.Device;
using Core.Contract.Commands.Events;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Aero.Application.Services;

public sealed class InputService(IInputRepository repo,IMessageBus bus) : IInputAdapter
{
      public async Task DeleteMonitorPointAsync(string mac, string ip, short deviceExternalId, short mpId, short slotNo, string metadata, CancellationToken ct = default)
      {
            var meta = JsonHelper.Deserialize<InputMetadata>(metadata);
            if (meta == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

            var res = repo.MonitorPointConfiguration(
                  mac,
                  deviceExternalId,
                  mpId,
                  -1,
                  slotNo,
                  meta.LfCode,
                  meta.LatchingMode,
                  meta.DelayEntry,
                  meta.DelayExit
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task MaskMonitorPointAsync(string mac, string ip, short deivceExternalId, short mpId, CancellationToken ct = default)
      {
           var res = repo.MonitorPointMask(
                  mac,
                  deivceExternalId,
                  mpId,
                  1
           );

           await bus.SendAsync(new AdapterEventCommand(res),ct);
      }

      public async Task MonitorPointAsync(
            string mac, 
            string ip, 
            short deviceExternalId,
            short moduleExternalId,
            short mpId,
            short slotNo,
            string metadata,
            CancellationToken ct = default)
      {
            var meta = JsonHelper.Deserialize<InputMetadata>(metadata);
            if (meta == null)
                  throw new Exception(MessageHelper.Common.DeserializeFailed("InputMetadata"));

            var res = repo.InputPointSpecification(
                  mac,
                  deviceExternalId,
                  moduleExternalId,
                  slotNo,
                  meta.Mode,
                  meta.Debounce,
                  meta.HoldTime
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct); 

            res = repo.MonitorPointConfiguration(
                  mac,
                  deviceExternalId,
                  mpId,
                  moduleExternalId,
                  slotNo,
                  meta.LfCode,
                  meta.LatchingMode,
                  meta.DelayEntry,
                  meta.DelayExit
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct); 
      }

      public async Task MonitorPointStatusAsync(string mac, string ip, short deivceExternalId, short mpId,short count, CancellationToken ct = default)
      {
            var res = repo.MonitorPointStatusRequest(
                  mac,
                  deivceExternalId,
                  mpId,
                  count
            );

            await bus.SendAsync(new AdapterEventCommand(res),ct); 
      }

      public async Task UnmaskMonitorPointAsync(string mac, string ip, short deivceExternalId, short mpId, CancellationToken ct = default)
      {
            var res = repo.MonitorPointMask(
                  mac,
                  deivceExternalId,
                  mpId,
                  0
           );

           await bus.SendAsync(new AdapterEventCommand(res),ct);
      }
}