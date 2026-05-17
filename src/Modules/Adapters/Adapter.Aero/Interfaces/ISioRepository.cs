using Adapter.Aero.Persistences.Entities;

public interface ISioRepository
{
      Task<SioPanelConfiguration> AddSioPanelConfigurationAsync(SioPanelConfiguration config ,CancellationToken ct = default);
      Task<SioPanelConfiguration> GetSioPanelConfigurationByModuleIdAsync(int ModuleId,CancellationToken ct = default);
      Task<bool> IsAnySioByScpIdAndSioIdAsync(int ScpId,int SioId,CancellationToken ct = default);
      Task<int> GetModuleIdByScpIdAndSioIdAsync(int ScpId,int ModuleId,CancellationToken ct = default);
      Task<int> GetModuleIdByMacAndSioNumberAsync(string Mac,short SioNo,CancellationToken ct = default);
      Task<short> GetSioNoByModuleIdAsync(int ModuleId,CancellationToken ct = default);
      Task<int> GetAeroIdByModuleIdAsync(int ModuleId);
}