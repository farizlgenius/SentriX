using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IDeviceModuleRepository : IBaseRepository<DeviceModuleDto, DeviceModule>
{
      Task<int> GetDeviceModuleIdByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<Dictionary<Guid, int>> GetDeviceModuleIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(int locationId, Vendor vedor, CancellationToken ct = default);
      Task<IEnumerable<DeviceModuleDto>> GetByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<(int TotalSlots, List<int> OccupiedSlots)?> GetReaderSlotAsync(Guid guid,CancellationToken ct = default);
      Task<(int TotalSlots, List<int> InputSlots,List<int> RexSlots,List<int> SensorSlots,List<int> BreakGlassSlots)?> GetInputSlotAsync(Guid guid, CancellationToken ct = default);
      Task<(int TotalSlots, List<int> OutputSlots,List<int> BuzzerSlots,List<int> RelaySlots)?> GetOutputSlotAsync(Guid guid,CancellationToken ct = default);
}