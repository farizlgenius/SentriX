using System;
using Device.Contract.DTOs;
using Device.Contract.Interfaces;

namespace Device.Application.Services;

public sealed class DeviceService() : IDevice
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            throw new NotImplementedException();
      }
}
