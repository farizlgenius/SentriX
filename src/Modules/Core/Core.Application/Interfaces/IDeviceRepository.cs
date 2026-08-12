using Core.Contract.DTOs.Device;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IDeviceRepository : IBaseRepository<DeviceDto, Device>
{

}