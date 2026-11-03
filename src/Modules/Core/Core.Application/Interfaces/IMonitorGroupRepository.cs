

using Core.Contract.DTOs.MonitorGroup;
using Microsoft.AspNetCore.Routing.Internal;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IMonitorGroupRepository : IRepository<MonitorGroupDto,Domain.Entities.MonitorGroup>
{
      Task<short> GetFreeExternalIdByMacAsync(string mac,int max,IEnumerable<short> excepions,CancellationToken ct = default);
      Task<IEnumerable<MonitorGroupDto>> GetByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<Guid> GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId,short deviceExternalId,Vendor vendor,CancellationToken ct = default);
      Task<IEnumerable<(Guid Guid,short ExternalId,MonitorType Type)>> GetComponentExternalIdMapGuidByMonitorGroupExternalIdAndDeviceExternalIdAndVendorAsync(short externalId, short deviceExternalId, Vendor vendor, CancellationToken ct = default);
      Task<string> GetNameByExternalIdAndMacAndVendorAsync(short ExternalId,string Mac,Vendor Vendor,CancellationToken ct = default);
}