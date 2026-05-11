using System;
using Device.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IDeviceAdapter
{
      Task<List<IdReportDto>> GetIdReportsAsync();
}
