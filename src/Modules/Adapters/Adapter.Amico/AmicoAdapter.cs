using System;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Interface;

namespace Adapter.Amico;

public sealed class AmicoAdapter : IAdapter
{
      public string Vendor => Abstraction.Constants.Vendor.AMICO;

      public IDeviceAdapter Device { get; }

      public IInputAdapter Monitor { get; }

      public IOutputAdapter Control { get; }

      public ITimeAdapter Time { get; }

      public IDoorAdapter Door { get; }
      public IGroupAdapter Group { get; }

      public IUserAdapter User { get; }
      public ISettingAdapter Setting { get; }

      public AmicoAdapter(
            IAmicoDeviceAdapter devices,
            IAmicoTimeAdapter times,
            IAmicoDoorAdapter doors,
            IAmicoUserAdapter users,
            IAmicoSettingAdapter settings
      )
      {
            Device = devices;
            Time = times;
            Door = doors;
            Setting = settings;
            User = users;

      }
}
