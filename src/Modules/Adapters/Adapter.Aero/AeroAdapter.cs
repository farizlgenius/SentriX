using System;
using Adapter.Abstraction.Interfaces;

namespace Adapter.Aero;

public sealed class AeroAdapter : IAdapter
{
      public string Vendor => "AERO";

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
