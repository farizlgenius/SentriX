
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IModuleAdapter
{
      Task GetStatusAsync(
           string mac,
            string ip,
            short deviceId,
            short moduleId,
            CancellationToken ct = default
      );
}