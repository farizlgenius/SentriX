using System;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;

namespace Adapter.Amico;

public sealed class AmicoAdapter : IAdapter
{
      public string Vendor => Venders.AMICO;

      public IDeviceAdapter Device { get; }

      public IMonitorAdapter Monitor { get; }

      public IControlAdapter Control { get; }

      public ITimeAdapter Time {get;}

      public IDoorAdapter Door {get;}
      public IGroupAdapter Group {get;}

      public AmicoAdapter(
            IDeviceAdapter devices,
            IMonitorAdapter monitors,
            IControlAdapter controls,
            ITimeAdapter time,
            IDoorAdapter door,
            IGroupAdapter group
      )
      {
            Device = devices;
            Monitor = monitors;
            Control = controls;
            Time = time;
            Door = door;
            Group = group;
      }
}
