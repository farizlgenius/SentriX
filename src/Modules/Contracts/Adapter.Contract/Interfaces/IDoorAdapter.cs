
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IDoorAdapter
{
      Task Doors(
            string mac,
            string ip,
            DoorType type,
            short deviceId,
            string metadata,
            List<(
                  short readerNo,
                  ReaderMode readerMode,
                  ReaderDirection readerDirection,
                  string metadata,
                  int deviceModuleId
            )> readers,
            (short outputNo,OutputMode mode,string metadata,int deviceModuleId)? buzzer,
            (short inputNo,InputMode mode,string metadata,int deviceModuleId)? rex,
            (short inputNo,InputMode mode,string metadata,int deviceModuleId)? bg,
            (short inputNo,OutputMode mode,string metadata,int deviceModuleId)? sensor,
            (short outputNo,OutputMode mode,string metadata,int deviceModuleId)? relay,
            CancellationToken ct = default
      );
}