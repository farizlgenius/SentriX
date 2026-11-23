using Core.Contract.DTOs.Monitor;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IMonitor : IService<MonitorDto,CreateMonitorDto,UpdateMonitorDto>
{
      Task<bool> GetStatusAsync(Guid guid,CancellationToken ct = default);
      Task<bool> MaskAsync(Guid guid,CancellationToken ct = default);
      Task<bool> UnmaskAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default);
}