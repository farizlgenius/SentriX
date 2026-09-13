using Core.Contract.DTOs.Company;
using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IComponentMapping
{
      Task<int?> GetFreeIdByMacAndEntityAndVendorAsync(string entity, Vendor vendor, int Max, CancellationToken ct = default);
}