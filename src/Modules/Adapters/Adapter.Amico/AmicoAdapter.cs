using System;
using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Interface;
using SharedKernel.Enums;

namespace Adapter.Amico;

public sealed class AmicoAdapter : IAdapter
{
      public Vendor Vendor => Vendor.amico;

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
