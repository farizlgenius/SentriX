using System;
using Adapter.Abstraction.Interfaces;
using Device.Contract.DTOs;
using Device.Contract.Interfaces;

namespace Device.Application.Behaviors;

public sealed class DeviceBehaviors(IAdapterFactory adapterFactory) : IDevice
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            var adapter = adapterFactory.GetAdapter("aero");
            return await adapter.Device.GetIdReportsAsync();
      }
}
