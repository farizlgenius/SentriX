using Core.Contract.DTOs;
using Core.Contract.DTOs.DeviceModule;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface IDeviceModuleRepository : IRepository<DeviceModuleDto, DeviceModule>
{
      Task<Dictionary<Guid, int>> GetDeviceModuleIdsMapGuidsByGuidsAsync(IEnumerable<Guid> guids, CancellationToken ct = default);
      Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(int locationId, Vendor vedor, CancellationToken ct = default);
      Task<IEnumerable<DeviceModuleDto>> GetByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<(short TotalSlots, List<short> OccupiedSlots)?> GetReaderSlotAsync(Guid guid, CancellationToken ct = default);
      Task<(short TotalSlots, List<short> InputSlots,List<short> RexSlots,List<short> SensorSlots,List<short> BreakGlassSlots)?> GetInputSlotAsync(Guid guid, CancellationToken ct = default);
      Task<(short TotalSlots, List<short> OutputSlots,List<short> BuzzerSlots,List<short> RelaySlots)?> GetOutputSlotAsync(Guid guid,CancellationToken ct = default);
       Task<Guid> GetGuidByExternalIdAndDeviceExternalIdAndVendorAsync(short externalId,short deviceExternalId,Vendor vendor,CancellationToken ct = default);
     
}