
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IDoorAdapter
{
      Task AddDoorsAsync(
           string mac,
            string ip,
            DoorType type,
            short deviceId,
            List<short> doorId,
            string metadata,
            List<(
                  short readerNo,
                  ReaderMode readerMode,
                  ReaderDirection readerDirection,
                  string metadata,
                  short deviceModuleId
            )> readers,
            (short outputNo,string metadata,short deviceModuleId,short buzzerId)? buzzer,
            List<(short inputNo,string metadata,short deviceModuleId,short maskId)> rexes,
            (short inputNo,string metadata,short deviceModuleId,short bgId)? bg,
            (short inputNo,string metadata,short deviceModuleId)? sensor,
            (short outputNo,string metadata,short deviceModuleId)? relay,
            CancellationToken ct = default
      );

      Task DeleteDoorsAsync(
           string mac,
            string ip,
            DoorType type,
            short deviceId,
            List<short> doorId,
            short? buzzerId = 0,
            short? bgId = 0,
            CancellationToken ct = default
      );

      Task StatusAsync(
            string mac,
            string ip,
            short deviceId,
            short doorId,
            CancellationToken ct = default
      );

      Task ChangeDoorModeAsync(
            string mac,
            string ip,
            short deviceId,
            short acrId,
            DoorMode mode,
            CancellationToken ct  =default
      );

      Task UnlockAsync(
            string mac,
            string ip,
            short deviceId,
            short acrId,
            CancellationToken ct = default
      );
}