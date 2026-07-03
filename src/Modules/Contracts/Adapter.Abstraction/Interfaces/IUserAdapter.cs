

using SharedKernel.Domain;
using Time.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IUserAdapter
{
     Task CreateUserAsync(
             string Mac, 
            short ScpId, 
            int Flags, 
            int CardNumber, 
            short IssueCode, 
            string Pin,
            List<short> Groups,
            short ApbLoc,
            short UseCount,
            int ActiveDate,
            int DeactiveDate,
            int VacDate,
            short VacDays,
            int TmpDate,
            short TmpDays
     );  


}