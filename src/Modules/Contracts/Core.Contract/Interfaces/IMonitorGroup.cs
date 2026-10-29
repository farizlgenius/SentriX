using Core.Contract.DTOs.MonitorGroup;

namespace Core.Contract.Interfaces;

public interface IMonitorGroup : IService<MonitorGroupDto,CreateMonitorGroupDto,UpdateMonitorGroupDto>
{
      Task<bool> ArmAsync(Guid guid,CancellationToken ct = default);
      Task<bool> DisarmAsync(Guid guid,CancellationToken ct = default);
}