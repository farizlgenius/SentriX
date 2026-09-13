using System;
using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using SharedKernel.Enums;

namespace Adapter.Aero;

public sealed class AeroAdapter : IAdapter
{
      public Vendor Vendor => Vendor.aero;

      public IDeviceAdapter Device { get; }

      public IInputAdapter Monitor { get; }

      public IOutputAdapter Control { get; }

      public ITimeAdapter Time { get; }
      public IDoorAdapter Door { get; }
      public IGroupAdapter Group { get; }

      public IUserAdapter User { get; }
      public ISettingAdapter Setting { get; }

      public AeroAdapter(
            IAeroDeviceAdapter devices,
            IAeroOutputAdapter controls,
            IAeroInputAdapter monitor,
            IAeroTimeAdapter time,
            IAeroDoorAdapter door,
            IAeroGroupAdapter group,
            IAeroUserAdapter user,
            IAeroSettingAdapter setting
      )
      {
            Device = devices;
            Control = controls;
            Monitor = monitor;
            Time = time;
            Door = door;
            Group = group;
            User = user;
            Setting = setting;
      }
}
