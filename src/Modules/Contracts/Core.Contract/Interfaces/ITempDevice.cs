

using Core.Contract.DTOs.Device;

namespace Core.Contract.Interfaces;

public interface ITempDevice
{
    bool TryGet(string macAddress, out TempDeviceDto? device);

    bool Contains(string macAddress);

    bool TryAdd(TempDeviceDto device);


    bool TryRemove(string macAddress);

    IReadOnlyCollection<TempDeviceDto> GetAll();

    int Count { get; }
}