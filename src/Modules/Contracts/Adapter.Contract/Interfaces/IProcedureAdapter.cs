namespace Adapter.Contract.Interfaces;

public interface IProcedureAdapter
{
     Task AddProcedureAsync(string mac, string ip,short deviceId,short procId, string metadata,CancellationToken ct = default);


}