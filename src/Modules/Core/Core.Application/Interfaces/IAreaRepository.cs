using Core.Contract.DTOs.Area;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IAreaRepository : IRepository<AreaDto, Domain.Entities.Area>
{
      Task<Dictionary<Guid,int>> GetAreaIdMapGuidByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<IEnumerable<AreaDto>> GetByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<string> GetNameByExternalIdAndMacAndVendorAsync(short ExternalId,string Mac,Vendor Vendor,CancellationToken ct = default);
      Task<Guid> GetGuidByExternalIdAndMacAndVendorAsync(short ExternalId,string Mac,Vendor vendor,CancellationToken ct = default);
}