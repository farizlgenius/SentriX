using System.Linq.Expressions;
using Core.Contract.DTOs.Company;
using Core.Contract.DTOs.Department;
using Core.Contract.DTOs.DeviceModule;

namespace Core.Infrastructure.Projections;

public static class DeviceModuleProjections
{
      public static readonly Expression<Func<Persistences.Entities.DeviceModule,DeviceModuleDto>> ToDto = 
      x => new DeviceModuleDto(
             x.guid,
                        x.name,
                        x.serial_number,
                        x.firmware,
                        x.mac,
                        x.port,
                        x.address,
                        x.model,
                        x.reader_slot,
                        x.output_slot,
                        x.input_slot,
                        x.device.guid,
                        x.device.name,
                        x.location.guid,
                        x.location.name,
                        x.is_active,
                        x.is_default
      );
}