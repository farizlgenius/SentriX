

using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IUserAdapter
{


     Task AddUserAsync(
          string Mac,
          short DeviceComponentId,
          string Identification,
          string Name,
          int Active,
          int Expire,
          int Card,          
          string License,
          string Pin,
          string QrCode,
          string FaceFile,
          List<short> Groups
     );

     Task DeleteUserAsync(
          string Mac, 
            short ScpId,
            int CardNumber,
            string LicenseNumber,
            string Pin,
            string QrCode,
            string ImageName
     );


}