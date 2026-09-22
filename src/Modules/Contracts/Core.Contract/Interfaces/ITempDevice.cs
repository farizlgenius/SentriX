

using Core.Contract.DTOs.Device;

namespace Core.Contract.Interfaces;

public interface ITempDevice
{
    bool TryGet(string macAddress, out TempDeviceDto? device);

    string TryGetMacById(int id);

    bool Contains(string macAddress);

    bool TryAdd(TempDeviceDto device);

    bool TryRemove(string macAddress);

    IReadOnlyCollection<TempDeviceDto> GetAll();
    void TryUpdatePort(int Id,int Port);
    void TryUpdateIp(int Id,string Ip);

    int Count { get; }

    IEnumerable<int> TryGetUnavailableId();
}