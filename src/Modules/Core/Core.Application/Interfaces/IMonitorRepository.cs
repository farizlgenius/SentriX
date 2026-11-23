using Core.Contract.DTOs.Monitor;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IMonitorRepository : IRepository<MonitorDto, Domain.Entities.Monitor>
{
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
      Task<string> GetNameByExternalIdAndMacAndVendorAsync(short externalId,string mac,Vendor vendor,CancellationToken ct = default);
      Task<Guid> GuidByExternalIdAndMacAndVendorAsync(short externalId,string mac,Vendor vendor,CancellationToken ct = default);

      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default);
      Task<IEnumerable<MonitorDto>> GetByDeviceAsync(Guid guid, CancellationToken ct = default);
}
