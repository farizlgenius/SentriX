using System;
using Device.Contract.DTOs;

namespace Device.Contract.Interfaces;

public interface IDevice
{
      Task<List<IdReportDto>> GetIdReportsAsync();
}
