using Adapter.Amico.Interface;
using Adapter.Amico.Interfaces;
using SharedKernel.Messaging;

namespace Adapter.Amico.Adapters;

public sealed class AmicoUserAdapter(
      IDeviceCommand command,
      IAmicoRepository repo,
      IMessageBus bus
      ) : IAmicoUserAdapter
{
      public Task CreateUserAsync(string Mac, short ScpId, int Flags, int CardNumber, short IssueCode, string Pin, List<short> Groups, short ApbLoc, short UseCount, int ActiveDate, int DeactiveDate, int VacDate, short VacDays, int TmpDate, short TmpDays)
      {
            throw new NotImplementedException();
      }

      public Task DeleteUserAsync(string Mac, short ScpId, int CardNumber)
      {
            throw new NotImplementedException();
      }
}