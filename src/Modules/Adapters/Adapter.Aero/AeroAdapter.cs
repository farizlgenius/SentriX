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

      public ITimeAdapter Time {get;}
      public IDoorAdapter Door {get;}
      public IGroupAdapter Group {get;}

      public AeroAdapter(
            IDeviceAdapter devices,
            IControlAdapter controls,
            IMonitorAdapter monitor,
            ITimeAdapter time,
            IDoorAdapter door,
            IGroupAdapter group
      )
      {
            Device = devices;
            Control = controls;
            Monitor = monitor;
            Time = time;
            Door = door;
            Group = group;
      }
}
