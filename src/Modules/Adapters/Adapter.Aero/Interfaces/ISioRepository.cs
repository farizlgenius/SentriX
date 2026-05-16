using Adapter.Aero.Persistences.Entities;

public interface ISioRepository
{
      Task<SioPanelConfiguration> AddSioPanelConfigurationAsync(SioPanelConfiguration config ,CancellationToken ct = default);
      Task<bool> IsAnySioByScpIdAndSioIdAsync(int ScpId,int SioId,CancellationToken ct = default);
      Task<SioPanelConfiguration> GetSioPanelConfigurationByIdAndMacAndAddressAsync(short ScpId,string Mac,short Address,CancellationToken ct=default);
      Task<int> GetModuleIdByScpIdAndSioIdAsync(int ScpId,int ModuleId);
}