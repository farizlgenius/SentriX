using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using Events.Contract.Command;
using SharedKernel.Messaging;

namespace Adapter.Aero.Adapters;

public sealed class AeroUserAdapter(IUserCommand user, IMessageBus bus) : IAeroUserAdapter
{
      public async Task CreateUserAsync(
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

      public async Task DeleteUserAsync(
            string Mac, 
            short ScpId,
            int CardNumber
            )
      {


            var res = user.CardDelete(
                  Mac,
                  ScpId,
                  CardNumber
                  );

            await bus.SendAsync(new AddCommandEvent(res));
      }


}