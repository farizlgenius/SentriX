
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IDoorAdapter
{
      Task DoorsAsync(
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

      Task StatusAsync(
            string mac,
            string ip,
            short deviceId,
            short doorId,
            CancellationToken ct = default
      );
}