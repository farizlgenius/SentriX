
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IDoorAdapter
{
      Task Doors(
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
                  int deviceModuleId
            )> readers,
            (short outputNo,string metadata,short deviceModuleId,short buzzerId)? buzzer,
            (short inputNo,string metadata,short deviceModuleId)? rex,
            (short inputNo,string metadata,short deviceModuleId,short bgId)? bg,
            (short inputNo,string metadata,short deviceModuleId)? sensor,
            (short outputNo,string metadata,short deviceModuleId)? relay,
            CancellationToken ct = default
      );
}