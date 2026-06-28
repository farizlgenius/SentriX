using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Adapter.Aero.Services;

public sealed class AeroUserService(IUserCommand user, IMessageBus bus) : IUserAdapter
{
      public async Task CreateUserAsync(
            string Mac, 
            short ScpId, 
            short Flags, 
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
            )
      {


            var res = user.AccessDatabaseCardRecords(
                  Mac,
                  ScpId,
                  Flags,
                  CardNumber,
                  IssueCode,
                  Pin,
                  Groups,
                  ApbLoc,
                  UseCount,
                  ActiveDate,
                  DeactiveDate,
                  VacDate,
                  VacDays,
                  TmpDate,
                  TmpDays
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }
}