namespace Adapter.Contract.Interfaces;

public interface IProcedureAdapter
{
     Task AddActionAsync(string mac, string ip,short deviceId,short procId, string metadata,CancellationToken ct = default);


}