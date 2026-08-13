using Core.Contract.DTOs.Company;

namespace Core.Contract.Interfaces;

public interface IComponentMapping
{
      Task<int> GetFreeIdByMacAndEntityAndVendorAsync(string mac,string entity,string vendor,CancellationToken ct = default);
}