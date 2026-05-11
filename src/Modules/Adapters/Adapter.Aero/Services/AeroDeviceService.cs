using System;
using Adapter.Abstraction;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Device.Contract.DTOs;

namespace Adapter.Aero.Services;

public sealed class AeroDeviceService(IIdReportService idReport) : IDeviceAdapter
{
      public async Task<List<IdReportDto>> GetIdReportsAsync()
      {
            return idReport.IdReportInMemory.Select(x => new IdReportDto(x.ScpId,x.SerialNumber,x.Mac,x.Ip,x.Port,x.Fw)).ToList();
      }
}
