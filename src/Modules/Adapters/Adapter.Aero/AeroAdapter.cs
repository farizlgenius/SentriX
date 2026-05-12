using System;
using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;

namespace Adapter.Aero;

public sealed class AeroAdapter : IAdapter
{
      public string Vendor => Venders.AERO;

      public IDeviceAdapter Device { get; }

      public IMonitorAdapter Monitor { get; }

      public IControlAdapter Control { get; }

      public AeroAdapter(
            IDeviceAdapter devices
            // IMonitorAdapter monitors,
            // IControlAdapter controls
      )
      {
            Device = devices;
            // Monitor = monitors;
            // Control = controls;
      }
}
