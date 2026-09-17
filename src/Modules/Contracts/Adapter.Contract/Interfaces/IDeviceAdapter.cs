using System;
using System.Text.Json;

namespace Adapter.Contract.Interfaces;

public interface IDeviceAdapter
{

      Task InititalDeviceAsync(
            string Mac,
            string Ip,
            CancellationToken ct = default
            );



}
