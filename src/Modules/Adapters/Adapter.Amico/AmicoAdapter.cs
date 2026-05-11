using System;
using Adapter.Abstraction.Interfaces;

namespace Adapter.Amico;

public sealed class AmicoAdapter : IAdapter
{
      public string Vendor => "AMICO";

      public IDeviceAdapter Device { get; }

      public IMonitorAdapter Monitor { get; }

      public IControlAdapter Control { get; }

      public AmicoAdapter(
            IDeviceAdapter devices,
            IMonitorAdapter monitors,
            IControlAdapter controls
      )
      {
            Device = devices;
            Monitor = monitors;
            Control = controls;
      }
}
