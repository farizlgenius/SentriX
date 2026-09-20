using System;
using System.Text.Json;
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IDeviceAdapter
{

      Task InititalDeviceAsync(
            string mac,
            string ip,
            CancellationToken ct = default
            );

      Task UploadAllConfigurationAsync(
            string mac,
            string ip,
            CancellationToken ct = default
      );

      Task GetConfigurationAsync(
            string mac,
            string ip,
            CancellationToken ct = default
      );

      Task ResetAsync(
            string mac,
            string ip,
            CancellationToken ct = default
      );

      Task<Status> GetStatusAsync(
            string mac,
            string ip,
            CancellationToken ct = default
      );



}
