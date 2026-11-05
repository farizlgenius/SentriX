using Core.Contract.DTOs.Input;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IInputRepository : IRepository<InputDto, Input>
{
      Task<short> GetFreeExeternalIdByMacAsync(string mac,int max, IEnumerable<short>? exception = null, CancellationToken ct = default);
      Task<string> GetNameByExternalIdAndMacAndVendorAsync(short externalId,string mac,Vendor vendor,CancellationToken ct = default);
      Task<Guid> GuidByExternalIdAndMacAndVendorAsync(short externalId,string mac,Vendor vendor,CancellationToken ct = default);

      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default);
      Task<IEnumerable<InputDto>> GetByDeviceAsync(Guid guid, CancellationToken ct = default);
}
