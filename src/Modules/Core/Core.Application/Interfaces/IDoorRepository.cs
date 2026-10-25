using Core.Contract.DTOs.Door;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IDoorRepository : IRepository<DoorDto, Door>
{
      Task<bool> CheckRelationAsync(Guid guid, CancellationToken ct = default);
      Task<Dictionary<Guid, int>> GetDoorIdsMapGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
     Task<IEnumerable<(Guid guid, int id, string mac, string ip, Vendor vendor,short external_id)>> GetDetailsByGuidAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<DoorDto>> GetByMacAsync(string mac,CancellationToken ct = default);
      Task<IEnumerable<int>> GetIdsByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
      Task<IEnumerable<short>> GetExternalIdRangeByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<string> GetNameByExternalIdAndMacAndVendorQuery(short externalId,string mac,Vendor vendor,CancellationToken ct = default);
      Task<Guid> GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId,short deviceExternalId,Vendor vendor,CancellationToken ct = default);

}