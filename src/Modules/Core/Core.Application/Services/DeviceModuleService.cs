using Core.Application.Interfaces;
using Core.Contract.DTOs.DeviceModule;
using Core.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class DeviceModuleService(IDeviceModuleRepository repo, IMessageBus bus) : IDeviceModule
{
  public async Task<Guid> CreateAsync(CreateDeviceModuleDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<DeviceModuleDto>> GetByDeviceAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetByDeviceAsync(guid, ct);
  }

  public async Task<DeviceModuleDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<DeviceModuleDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<DeviceModuleDto>> GetByVendorAndLocationAsync(Guid locationGuid, Vendor vendor, CancellationToken ct = default)
  {

    ValidationHelper.Vendor(vendor);

    var locationId = await bus.QueryAsync(new LocationIdByGuidQuery(locationGuid));

    return await repo.GetByVendorAndLocationAsync(locationId, vendor, ct);

  }

  public async Task<IEnumerable<OptionDto>> GetInputSlotAsync(Guid guid, CancellationToken ct = default)
  {
    // 1. Get the data from the repository
        var slotData = await repo.GetInputSlotAsync(guid, ct);

        if (slotData == null)
            return Array.Empty<OptionDto>();

        // 2. Apply business logic: Calculate free slots
        var allPossibleSlots = Enumerable.Range(0, slotData.Value.TotalSlots);

        var occupiedSlots = slotData.Value.InputSlots.Concat(slotData.Value.RexSlots).Concat(slotData.Value.SensorSlots).Concat(slotData.Value.BreakGlassSlots).Distinct();
        
        return allPossibleSlots
            .Except(occupiedSlots)
            .Order()
            .Select(x => new OptionDto(
              $"Input {x+1}",
              x,
              string.Empty
            ))
            .ToArray();

  }

  public async Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid, CancellationToken ct = default)
  {
    return await repo.GetOptionByDeviceAsync(guid, ct);
  }

  public async Task<IEnumerable<OptionDto>> GetOutputSlotAsync(Guid guid, CancellationToken ct = default)
  {
    // 1. Get the data from the repository
        var slotData = await repo.GetOutputSlotAsync(guid, ct);

        if (slotData == null)
            return Array.Empty<OptionDto>();

        // 2. Apply business logic: Calculate free slots
        var allPossibleSlots = Enumerable.Range(0, slotData.Value.TotalSlots);

        var occupiedSlots = slotData.Value.RelaySlots.Concat(slotData.Value.BuzzerSlots).Concat(slotData.Value.OutputSlots).Distinct();
        
        return allPossibleSlots
            .Except(occupiedSlots)
            .Order()
            .Select(x => new OptionDto(
              $"Output {x+1}",
              x,
              string.Empty
            ))
            .ToArray();
  }

  public async Task<Pagination<DeviceModuleDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<OptionDto>> GetReaderSlotAsync(Guid guid, CancellationToken ct = default)
  {
     // 1. Get the data from the repository
        var slotData = await repo.GetReaderSlotAsync(guid, ct);

        if (slotData == null)
            return Array.Empty<OptionDto>();

        // 2. Apply business logic: Calculate free slots
        var allPossibleSlots = Enumerable.Range(0, slotData.Value.TotalSlots);

        var occupiedSlots = slotData.Value.OccupiedSlots.Distinct();
        
        return allPossibleSlots
            .Except(occupiedSlots)
            .Order()
            .Select(x => new OptionDto(
              $"Reader {x+1}",
              x,
              string.Empty
            ))
            .ToArray();
  }

  public async Task<Guid> UpdateAsync(UpdateDeviceModuleDto dto, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public Task UploadAsync(CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}