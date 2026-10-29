

using Core.Contract.DTOs.MonitorGroup;
using Microsoft.AspNetCore.Routing.Internal;

namespace Core.Application.Interfaces;

public interface IMonitorGroupRepository : IRepository<MonitorGroupDto,Domain.Entities.MonitorGroup>
{
      Task<short> GetFreeExternalIdByMacAsync(string mac,int max,IEnumerable<short> excepions,CancellationToken ct = default);
}