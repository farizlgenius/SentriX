namespace Adapter.Contract.Interfaces;

public interface IGroupAdapter
{
     Task CreateAccessGroupAsync(string mac, string ip,short deviceId,short groupId, List<(short doorId, short timeZoneId)> datas,CancellationToken ct = default);

}