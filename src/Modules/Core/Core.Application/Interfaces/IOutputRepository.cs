using Core.Contract.DTOs.Output;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IOutputRepository : IRepository<OutputDto, Output>
{
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
      Task<Guid> OutputGuidByExternalIdAndMacAndVendorAsync(short externalId,string mac,Vendor vendor,CancellationToken ct = default);
      Task<string> GetNameByExternalIdAndMacAndVendorAsync(short externalId,string mac,Vendor vendor,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default);
      Task<IEnumerable<OutputDto>> GetByDeviceAsync(Guid guid, CancellationToken ct = default);
}