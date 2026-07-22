

using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IUserAdapter
{


     Task CreateAsync(
           Guid DeviceGuid,
            string Identification,
            string Name,
            int Active,
            int Expire,
            int Card,
            string LicensePlate,
            string Pin,
            string QrCode,
            string FaceFile,
            List<Guid> Groups
     );

     Task DeleteAsync(
         Guid DeviceGuid, int CardNumber, string LicenseNumber, string Pin, string QrCode, string ImageName
     );


}