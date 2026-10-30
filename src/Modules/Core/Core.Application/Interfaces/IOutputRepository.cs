using Core.Contract.DTOs.Output;
using Core.Domain.Entities;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IOutputRepository : IRepository<OutputDto, Output>
{
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
      Task<Guid> OutputGuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId,short deviceExternalId,Vendor vendor,CancellationToken ct = default);
}