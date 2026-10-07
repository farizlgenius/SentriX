namespace Adapter.Contract.Interfaces;

public interface IGroupAdapter
{
     Task AddAccessGroupAsync(string mac, string ip,short deviceId,short groupId, List<(short doorId, short timeZoneId)> datas);

}