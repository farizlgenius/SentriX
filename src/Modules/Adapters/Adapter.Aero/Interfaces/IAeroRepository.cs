using Adapter.Aero.Persistences.Entities;
using SharedKernel.Domain;

namespace Adapter.Aero.Interfaces;

public interface IAeroRepository
{
      Task<ScpDeviceSpecification> GetScpDeviceSpecificationAsync(CancellationToken ct = default);
      Task<AccessDatabaseSpecification> GetAccessDatabaseSpecificationAsync(CancellationToken ct = default);
      Task<ElevatorAccessLevelSpecification> GetElevatorAccessLevelSpecificationAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetRelayOptionAsync(CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(CancellationToken ct = default);

      // Create Slot 
      Task AddSlotAsync<TEntity>(
            Guid guid,
            int slot,
             Func<Guid, int, TEntity> factory,
            CancellationToken ct = default
            ) where TEntity : BaseSlot;

       Task AddCentrlSlotAsync<TEntity>(
            int slot,
             Func<int, TEntity> factory, 
            CancellationToken ct = default
            ) where TEntity : CentralBaseSlot;


      // Insert Slot
      Task InsertSlotAsync<TEntity>(
            Guid device_guid,
            Guid module_guid,
            int slot,
            CancellationToken ct = default
            ) where TEntity : BaseSlot;

      Task InsertCentralSlotAsync<TEntity>(
            Guid module_guid,
            int slot,
            CancellationToken ct = default
            ) where TEntity : CentralBaseSlot;

      // Eject Sloe
      Task EjectSlotAsync<TEntity>(
            Guid guid,
            int slot,
            CancellationToken ct = default
            ) where TEntity : BaseSlot;

      Task EjectCentralSlotAsync<TEntity>(
            int slot,
            CancellationToken ct = default
            ) where TEntity : CentralBaseSlot;

      // Get Free Slot

       Task<int> GetFreeSlotAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot;
       Task<int> GetCentralFreeSlotAsync<TEntity>(CancellationToken ct = default) where TEntity : CentralBaseSlot;


      Task<TEntity> GetSlotByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot;
      Task DeleteSlotAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : BaseSlot;
     

      // Central Slot Without device scope
      
      Task<TEntity> GetCentralSlotByGuidAsync<TEntity>(Guid guid, CancellationToken ct = default) where TEntity : CentralBaseSlot;
     

      Task<int> GetScpSlotByMacAsync(string mac, CancellationToken ct = default);
      Task InsertScpSlotAsync(Guid guid, string Mac, int slot, CancellationToken ct = default);
      Task<Guid> GetScpGuidBySlotAsync(int slot, CancellationToken ct = default);
      Task<ScpSlot> GetScpSlotByGuidAsync(Guid guid, CancellationToken ct = default);

      Task EjectScpSlotAsync(Guid guid, CancellationToken ct = default);




}